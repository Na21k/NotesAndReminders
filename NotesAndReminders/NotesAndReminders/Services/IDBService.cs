using NotesAndReminders.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesAndReminders.Services
{
	public interface IDBService
	{
		Task<bool> AddNoteAsync(Note note);
		Task<bool> DeleteNoteAsync(Note note, bool isArchiving);
		Task<bool> UpdateNoteAsync(Note note);
		Task<bool> AddNoteTypeAsync(NoteType noteType);
		Task<bool> DeleteNoteTypeAsync(NoteType noteType);
		Task<bool> UpdateNoteTypeAsync(NoteType noteType);
		Task GetNoteAsync(string noteId, Action<IDBItem> onNoteRecievedCallback);
		//Task GetAllNotesAsync(Action<List<IDBItem>> onNotesRecievedCallback);
		Task GetNoteTypeAsync(string noteTypeId, Action<IDBItem> onNoteTypeRecievedCallback);
		Task GetAllNoteTypesAsync(Action<List<IDBItem>> onNotesTypeRecievedCallback);
		Task GetAllArchivedNotesAsync(Action<List<IDBItem>> onNotesRecievedCallback);
		Task<bool> ArchiveNoteAsync(Note note);
		Task<bool> UnarchiveNoteAsync(Note note);
		Task<List<IDBItem>> GetAllNotes();
		int CreateNotification(Note note);
	}
}
