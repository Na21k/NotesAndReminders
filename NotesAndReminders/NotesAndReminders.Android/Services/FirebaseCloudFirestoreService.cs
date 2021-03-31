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
using NotesAndReminders.Models;
using NotesAndReminders.Services;
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
					{ "id", note.Id},
					{ "user_Id", _auth.CurrentUser.Uid},
					{ "title", note.Title },
					{ "text", note.Text},
					{ "type", note.Type},
					{ "addition content", note.Images },
					{ "checklist", note.Checklists},
					{ "last_time_modifired", note.LastEdited}
				};

				await docRef.Set(new HashMap(noteDoc));

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

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

				await docRef.Set(new HashMap(noteTypeDoc));

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

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
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

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
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async Task<List<Note>> GetAllNotesAsync()
		{
			//Query allNotesQuery = db.Collection("Notes");

			//QuerySnapshot allNotesQuerySnapshot = await allNotesQuery.Get(new HashMap());

			throw new NotImplementedException();
		}

		public Task<List<NoteType>> GetAllNoteTypesAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<Note> GetNoteAsync(string noteId)
		{
			//DocumentReference docRef = db.Collection("Notes").Document(noteId);
			//try
			//{
			//	DocumentSnapshot doc = (DocumentSnapshot)await docRef.Get();

			//}
			//catch
			//{

			//}

			throw new NotImplementedException();
		}

		public Task<NoteType> GetNoteTypeAsync(string noteTypeId)
		{
			throw new NotImplementedException();
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
					{ "type", note.Type},
					{ "addition content", note.Images },
					{ "checklist", note.Checklists},
					{ "last_time_modifired", note.LastEdited}
				};

				await docRef.Update((IDictionary<string, Java.Lang.Object>)updatedNote);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

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

				await docRef.Update((IDictionary<string, Java.Lang.Object>)updatedNoteType);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				throw;
			}
		}
	}
}
