using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
		public Task<bool> AddNoteAsync(Note note)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddNoteTypeAsync(NoteType noteType)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteNoteAsync(Note note)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteNoteTypeAsync(NoteType noteType)
		{
			throw new NotImplementedException();
		}

		public Task<List<Note>> GetAllNotesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<List<NoteType>> GetAllNoteTypesAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Note> GetNoteAsync(string noteId)
		{
			throw new NotImplementedException();
		}

		public Task<NoteType> GetNoteTypeAsync(string noteTypeId)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateNoteAsync(Note note)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateNoteTypeAsync(NoteType noteType)
		{
			throw new NotImplementedException();
		}
	}
}
