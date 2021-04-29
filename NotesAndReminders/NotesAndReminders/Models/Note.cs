using System;
using System.Collections.Generic;

namespace NotesAndReminders.Models
{
	public class Note : Identifiable, IDBItem
	{
		[Newtonsoft.Json.JsonProperty("title")]
		public string Title { get; set; }
		[Newtonsoft.Json.JsonProperty("text")]
		public string Text { get; set; }
		[Newtonsoft.Json.JsonProperty("type")]
		public NoteType Type { get; set; }
		[Newtonsoft.Json.JsonProperty("image")]
		public List<byte[]> Images { get; set; }
		[Newtonsoft.Json.JsonProperty("checklist")]
		public List<ChecklistItem> Checklists { get; set; }
		[Newtonsoft.Json.JsonProperty("state")]
		public NoteState State { get; set; }
		[Newtonsoft.Json.JsonProperty("last_time_modifired")]
		public DateTime LastEdited { get; set; }
	}
}
