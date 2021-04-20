using System;
using System.Collections.Generic;

namespace NotesAndReminders.Models
{
	public class Note : Identifiable, IDBItem
	{
		public string Title { get; set; }
		public string Text { get; set; }
		public NoteType Type { get; set; }
		public List<byte[]> Images { get; set; }
		public List<ChecklistItem> Checklists { get; set; }
		public NoteState State { get; set; }
		public DateTime LastEdited { get; set; }
	}
}
