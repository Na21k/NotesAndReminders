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
using NotesAndReminders.Droid.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseCloudFirestoreService))]
namespace NotesAndReminders.Droid.Services
{
	public class FirebaseCloudFirestoreService : IDBService
	{
		private FirebaseFirestore _db = FirebaseFirestore.Instance;

		private FirebaseAuth _auth = FirebaseAuth.Instance;

		public async Task<bool> AddNoteAsync(Note note)
		{
			try
			{
				DocumentReference docRef = _db.Collection("Notes").Document();
				Dictionary<string, object> noteDoc = new Dictionary<string, object>
				{
					{ "id", docRef.Id},
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "title", note.Title },
					{ "text", note.Text},
					//{ "type", note.Type},
					//{ "addition content", note.Images },
					//{ "checklist", note.Checklists},
					{ "last_time_modifired", note.LastEdited}
				};

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

		public async Task<bool> AddNoteTypeAsync(NoteType noteType)
		{
			try
			{
				DocumentReference docRef = _db.Collection("Notes").Document();
				Dictionary<string, object> noteTypeDoc = new Dictionary<string, object>
				{
					{ "id", noteType.Id },
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "name", noteType.Name},
					{ "noteColor", noteType.Color}
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

		public async Task<bool> DeleteNoteAsync(Note note)
		{
			try
			{
				DocumentReference docRef = _db.Collection("Notes").Document(note.Id.ToString());
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
				DocumentReference docRef = _db.Collection("Notes").Document(noteType.Id.ToString());
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

		public async Task GetAllNotesAsync(Action<List<IDBItem>> onNotesRecievedCallback)
		{
			try
			{
				var query = _db.Collection("Notes");
				await query.WhereEqualTo("user_Id", _auth.CurrentUser.Uid).Get().AddOnCompleteListener(new OnCompleteListListener<Note>(onNotesRecievedCallback));

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

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

		public async Task GetNoteAsync(string noteId, Action<IDBItem> onNoteRecievedCallback)
		{
			DocumentReference docRef = _db.Collection("Notes").Document(noteId);
			try
			{
				await docRef.Get().AddOnCompleteListener(new OnCompleteListener<Note>(onNoteRecievedCallback));
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}

		}

		public async Task GetNoteTypeAsync(string noteTypeId, Action<IDBItem> onNoteTypeRecievedCallback)
		{
			DocumentReference docRef = _db.Collection("NotesTypes").Document(noteTypeId);
			try
			{
				await docRef.Get().AddOnCompleteListener(new OnCompleteListener<NoteType>(onNoteTypeRecievedCallback));
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<bool> UpdateNoteAsync(Note note)
		{
			DocumentReference docRef = _db.Collection("Notes").Document(note.Id);
			try
			{
				Dictionary<string, object> updatedNote = new Dictionary<string, object>()
				{
					{ "id", note.Id},
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "title", note.Title },
					{ "text", note.Text},
					//{ "type", note.Type},
					//{ "addition content", note.Images },
					//{ "checklist", note.Checklists},
					{ "last_time_modifired", note.LastEdited}
				};

				await docRef.Update(updatedNote.Convert());

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
			DocumentReference docRef = _db.Collection("Notes").Document(noteType.Id);
			try
			{
				Dictionary<string, object> updatedNoteType = new Dictionary<string, object>()
				{
					{ "id", noteType.Id },
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "name", noteType.Name},
					{ "noteColor", noteType.Color}
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
	}
}
