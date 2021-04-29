using Newtonsoft.Json;
using NotesAndReminders.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders.Services
{
	public class TrashService
	{
		private IDBService _dBService;
		private IAuthorizationService _authorizationService;
		private const string _fileName = "TrashedNotes.bin";
		private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _fileName);

		public TrashService()
		{
			_dBService = DependencyService.Get<IDBService>();
			_authorizationService = DependencyService.Get<IAuthorizationService>();
		}

		public async Task<List<Note>> GetTrashedNotesAsync()
		{
			var binFormatter = new BinaryFormatter();

			try
			{
				var notes = new List<Note>();

				using (var fs = new FileStream(_path, FileMode.Open))
				{
					while (fs.Position < fs.Length)
					{
						var noteJson = await Task.Run(() => (string)binFormatter.Deserialize(fs));
						var note = JsonConvert.DeserializeObject<Note>(noteJson);
						notes.Add(note);
					}

					return notes;
				}
			}
			catch (FileNotFoundException)
			{
				return new List<Note>();
			}
		}

		public async Task MoveNoteToTrashAsync(Note note)
		{
			EnsureIsLoggedIn();

			if (note.State != NoteState.Regular && note.State != NoteState.Archived)
			{
				throw new Exception($"Invalid note state ({note.State})");
			}

			if (!await _dBService.DeleteNoteAsync(note))
			{
				throw new Exception("Failed to delete the note ftom db");
			}

			switch (note.State)
			{
				case NoteState.Regular:
					note.State = NoteState.Trashed;
					break;
				case NoteState.Archived:
					note.State = NoteState.ArchivedTrashed;
					break;
			}

			var allTrashedNotes = await GetTrashedNotesAsync();
			allTrashedNotes.Add(note);
			await OverwriteWithNewList(allTrashedNotes);
		}

		public async Task RestoreNoteFromTrashAsync(Note note)
		{
			EnsureIsLoggedIn();

			if (note.State != NoteState.Trashed && note.State != NoteState.ArchivedTrashed)
			{
				throw new Exception($"Invalid note state ({note.State})");
			}

			var allTrashedNotes = await GetTrashedNotesAsync();

			if (allTrashedNotes.Exists(n => n.Id == note.Id))
			{
				allTrashedNotes.RemoveAll(n => n.Id == note.Id);
				await OverwriteWithNewList(allTrashedNotes);

				//var savedNoteState = note.State;
				note.State = NoteState.Regular;
				await _dBService.AddNoteAsync(note);

				//duplucates in notes and archive folders, probably because of incorrect id
				/*if (savedNoteState == NoteState.ArchivedTrashed)
				{
					await _dBService.ArchiveNoteAsync(note);
				}*/
			}
			else
			{
				throw new Exception("There is no such note in the trash");
			}
		}

		public async Task DeleteNoteFromTrash(Note note)
		{
			if (note.State != NoteState.Trashed && note.State != NoteState.ArchivedTrashed)
			{
				throw new Exception($"Invalid note state ({note.State})");
			}

			var allTrashedNotes = await GetTrashedNotesAsync();

			if (allTrashedNotes.Exists(n => n.Id == note.Id))
			{
				allTrashedNotes.RemoveAll(n => n.Id == note.Id);
				await OverwriteWithNewList(allTrashedNotes);
			}
			else
			{
				throw new Exception("There is no such note in the trash");
			}
		}

		public void EmptyTrash()
		{
			File.Delete(_path);
		}

		private async Task OverwriteWithNewList(List<Note> notes)
		{
			var binFormatter = new BinaryFormatter();

			using (var fs = new FileStream(_path, FileMode.Create))
			{
				foreach (var note in notes)
				{
					var noteJson = JsonConvert.SerializeObject(note);
					await Task.Run(() => binFormatter.Serialize(fs, noteJson));
				}
			}
		}

		private void EnsureIsLoggedIn()
		{
			if (!_authorizationService.IsLoggedIn)
			{
				throw new Exception("User must be logged in");
			}
		}
	}
}
