using NotesAndReminders.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotesAndReminders.Services
{
	public interface IDBService
	{
		Task<bool> AddNoteAsync(Note note);
		Task<bool> DeleteNoteAsync(Note note);
		Task<bool> UpdateNoteAsync(Note note);
		Task<bool> AddNoteTypeAsync(NoteType noteType);
		Task<bool> DeleteNoteTypeAsync(NoteType noteType);
		Task<bool> UpdateNoteTypeAsync(NoteType noteType);
		Task<Note> GetNoteAsync(string noteId);
		Task<List<Note>> GetAllNotesAsync();
		Task<NoteType> GetNoteTypeAsync(string noteTypeId);
		Task<List<NoteType>> GetAllNoteTypesAsync();
	}
}
