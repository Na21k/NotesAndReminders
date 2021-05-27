using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using NotesAndReminders.Droid.Services;
using NotesAndReminders.Exceptions;
using NotesAndReminders.Models;
using NotesAndReminders.Services;
using NotesAndReminders.Resources;
using NotesAndReminders.Droid.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using Firebase.Storage;
using System.Threading;

[assembly: Dependency(typeof(FirebaseCloudFirestoreService))]
namespace NotesAndReminders.Droid.Services
{

	public class FirebaseCloudFirestoreService : IDBService
	{
		private FirebaseFirestore _db = FirebaseFirestore.Instance;

		private FirebaseAuth _auth = FirebaseAuth.Instance;

		INotificationManager notificationManager = DependencyService.Get<INotificationManager>();

		public async Task<bool> AddNoteAsync(Note note)
		{
			try
			{
				DocumentReference docRef = _db.Collection("Notes").Document();
				Dictionary<string, object> noteDoc = new Dictionary<string, object>();

				var imgUrls = new Dictionary<string, string>();
				if (note.Images != null)
				{
					imgUrls = await StoreImages(note.Images, docRef.Id);
				}

				if(note.NotificationTime != null)
				{
					notificationManager.SendNotification(note.Title, note.Text, note.NotificationTime);
				}


				
				if (note.Type == null && note.NotificationTime == null)
				{

					noteDoc = new Dictionary<string, object>
					{
						{ "id", docRef.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "state", NoteState.Regular.ToString()},
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited}
					};
				}
				else if (note.Type == null)
				{
					noteDoc = new Dictionary<string, object>
					{
						{ "id", docRef.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "state", NoteState.Regular.ToString()},
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "notification_time",  note.NotificationTime}
					};
				}
				else if (note.NotificationTime == null)
				{
					noteDoc = new Dictionary<string, object>
					{
						{ "id", docRef.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "state", NoteState.Regular.ToString()},
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "typeId", note.Type.Id}
					};
				}
				else
				{
					noteDoc = new Dictionary<string, object>
					{
						{ "id", docRef.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "state", NoteState.Regular.ToString()  },
						{ "typeId", note.Type.Id},
						{ "addition_content", imgUrls},
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "notification_time",  note.NotificationTime}
					};

					
				}

				await docRef.Set(noteDoc.Convert());
				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}

		}

		public async Task<Dictionary<string, string>> StoreImages(Dictionary<string, byte[]> imageStream, string noteId)
		{
			try
			{
				var result = new Dictionary<string, string>();
				foreach (var image in imageStream)
				{
					Stream stream = new MemoryStream(image.Value);
					var imageUrl = await new FirebaseStorage("notesandreminders-5c2a6.appspot.com")
						.Child(noteId)
						.Child(image.Key)
						.PutAsync(stream);

					result.Add(image.Key, imageUrl);
				}

				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task DeleteImage(string imageName, string noteId, Dictionary<string, byte[]> images)
		{
			try
			{
				if (imageName == "DeleteFolder")
				{
					foreach (var image in images)
					{
						await new FirebaseStorage("notesandreminders-5c2a6.appspot.com")
						.Child(noteId)
						.Child(image.Key)
						.DeleteAsync();
					}

				}
				else
				{
					await new FirebaseStorage("notesandreminders-5c2a6.appspot.com")
						.Child(noteId)
						.Child(imageName)
						.DeleteAsync();
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> AddNoteTypeAsync(NoteType noteType)
		{
			try
			{
				DocumentReference docRef = _db.Collection("NotesTypes").Document();
				Dictionary<string, object> noteTypeDoc = new Dictionary<string, object>
				{
					{ "id", noteType.Id },
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "name", noteType.Name},
					{ "noteColorLight", noteType.Color.Light.ToHex()},
					{ "noteColorDark", noteType.Color.Dark.ToHex()}
				};

				await docRef.Set(noteTypeDoc.Convert());

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> DeleteNoteAsync(Note note, bool isArchiving)
		{
			try
			{
				DocumentReference docRef;
				string colName = null;
				switch (note.State)
				{
					case NoteState.Regular:
						colName = "Notes";
						break;

					case NoteState.Archived:
						colName = "Archive";
						break;
				}

				docRef = _db.Collection(colName).Document(note.Id.ToString());
				if (isArchiving != true && note.Images != null)
				{
					await DeleteImage("DeleteFolder", note.Id, note.Images);
				}

				await docRef.Delete();

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> DeleteNoteTypeAsync(NoteType noteType)
		{
			try
			{
				var notesList = await FindNotesAsync(noteType);

				await DeleteNoteWithType(notesList);

				DocumentReference docRef = _db.Collection("NotesTypes").Document(noteType.Id.ToString());
				await docRef.Delete();

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<List<Note>> FindNotesAsync(NoteType noteType)
		{
			try
			{
				var notesList = new List<Note>();
				string noteTypeId = noteType.Id;

				var item = await GetAllNotes();

				item.ForEach(note =>
				{
					var nt = note as Note;
					if (nt.Type.Id == noteType.Id)
					{
						notesList.Add(nt);
					}
				});

				return notesList;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task DeleteNoteWithType(List<Note> notesList)
		{
			try
			{
				foreach (var note in notesList)
				{
					note.Type = null;
					note.NoteTypeId = null;

					await DeleteNoteAsync(note, false);
					await AddNoteAsync(note);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);
			}
		}
		public Task<List<IDBItem>> GetAllNotes()
		{
			try
			{
				var tcs = new TaskCompletionSource<List<IDBItem>>();

				var query = _db.Collection("Notes");
				query.WhereEqualTo("user_Id", _auth.CurrentUser.Uid).Get().AddOnCompleteListener(new Extensions.TaskListenere.OnCompleteListListener<Note>(tcs));

				return tcs.Task;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		//public async Task GetAllNotesAsync(Action<List<IDBItem>> onNotesRecievedCallback)
		//{
		//	try
		//	{
		//		var query = _db.Collection("Notes");
		//		await query.WhereEqualTo("user_Id", _auth.CurrentUser.Uid).Get().AddOnCompleteListener(new OnCompleteListListener<Note>(onNotesRecievedCallback));

		//	}
		//	catch (Exception ex)
		//	{
		//		System.Diagnostics.Debug.WriteLine(ex.Message);
		//		System.Diagnostics.Debug.WriteLine(ex.StackTrace);

		//		throw;
		//	}
		//}


		public async Task GetAllNoteTypesAsync(Action<List<IDBItem>> onNotesTypeRecievedCallback)
		{
			try
			{
				var query = _db.Collection("NotesTypes");
				await query.WhereEqualTo("user_Id", _auth.CurrentUser.Uid).Get().AddOnCompleteListener(new OnCompleteListListener<NoteType>(onNotesTypeRecievedCallback));
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task GetAllArchivedNotesAsync(Action<List<IDBItem>> onNotesRecievedCallback)
		{
			try
			{
				var query = _db.Collection("Archive");
				await query.WhereEqualTo("user_Id", _auth.CurrentUser.Uid).Get().AddOnCompleteListener(new OnCompleteListListener<Note>(onNotesRecievedCallback));

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}
		public async Task GetNoteAsync(string noteId, Action<IDBItem> onNoteRecievedCallback)
		{
			try
			{
				DocumentReference docRef = _db.Collection("Notes").Document(noteId);
				await docRef.Get().AddOnCompleteListener(new OnCompleteListener<Note>(onNoteRecievedCallback));

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}

		}

		public async Task GetNoteTypeAsync(string noteTypeId, Action<IDBItem> onNoteTypeRecievedCallback)
		{
			try
			{
				DocumentReference docRef = _db.Collection("NotesTypes").Document(noteTypeId);
				var task = docRef.Get();
				var listener = new OnCompleteListener<NoteType>(onNoteTypeRecievedCallback);
				await task.AddOnCompleteListener(listener);

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> UpdateNoteAsync(Note note)
		{
			try
			{
				DocumentReference docRef;
				string colName = null;
				switch (note.State)
				{
					case NoteState.Regular:
						colName = "Notes";
						break;

					case NoteState.Archived:
						colName = "Archive";
						break;
				}

				docRef = _db.Collection(colName).Document(note.Id);

				var imgUrls = new Dictionary<string, string>();
				if (note.Images != null)
				{
					imgUrls = await StoreImages(note.Images, note.Id);
				}
				if (note.NotificationTime != null)
				{
					notificationManager.SendNotification(note.Title, note.Text, note.NotificationTime);
				}


				if (note.Type == null || note.Type.Name == "Uncategorized")
				{
					Dictionary<string, object> updatedNote = new Dictionary<string, object>
					{
						{ "id", docRef.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "addition_content", imgUrls},
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "notification_time",  note.NotificationTime}
					};

					await DeleteNoteAsync(note, false);
					await docRef.Set(updatedNote.Convert());
				}
				else
				{
					Dictionary<string, object> updatedNote = new Dictionary<string, object>()
					{
						{ "id", note.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "typeId", note.Type.Id},
						{ "addition_content", imgUrls },
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "notification_time",  note.NotificationTime}
					};

					await docRef.Update(updatedNote.Convert());
				}


				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> UpdateNoteTypeAsync(NoteType noteType)
		{
			DocumentReference docRef = _db.Collection("NotesTypes").Document(noteType.Id);
			try
			{
				Dictionary<string, object> updatedNoteType = new Dictionary<string, object>()
				{
					{ "id", noteType.Id },
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "name", noteType.Name},
					{ "noteColorLight", noteType.Color.Light.ToHex()},
					{ "noteColorDark", noteType.Color.Dark.ToHex()}
				};

				await docRef.Update(updatedNoteType.Convert());

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> ArchiveNoteAsync(Note note)
		{
			try
			{
				DocumentReference docRef = _db.Collection("Archive").Document();

				var imgUrls = new Dictionary<string, string>();
				if (note.Images != null)
				{
					imgUrls = await StoreImages(note.Images, note.Id);
				}
				if (note.Type == null && note.Type.Name == "Uncategorized")
				{
					Dictionary<string, object> noteDoc = new Dictionary<string, object>
					{
						{ "id", docRef.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "state", NoteState.Archived.ToString()  },
						{ "addition_content", imgUrls},
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "notification_time",  note.NotificationTime}
					};

					await docRef.Set(noteDoc.Convert());
				}
				else
				{
					Dictionary<string, object> noteDoc = new Dictionary<string, object>()
					{
						{ "id", note.Id},
						{ "user_Id", _auth.CurrentUser.Uid},
						{ "title", note.Title },
						{ "text", note.Text},
						{ "state", NoteState.Archived.ToString()  },
						{ "typeId", note.Type.Id},
						{ "addition_content", imgUrls },
						{ "checklist", note.Checklist},
						{ "last_time_modifired", note.LastEdited},
						{ "notification_time",  note.NotificationTime}
					};

					await docRef.Set(noteDoc.Convert());
				}

				await DeleteNoteAsync(note, true);
				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> UnarchiveNoteAsync(Note note)
		{
			try
			{
				await AddNoteAsync(note);
				await DeleteNoteAsync(note, true);

				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

	}
}
