using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NotesAndReminders.Models
{
	public class Note : Identifiable, IDBItem
	{
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("text")]
		public string Text { get; set; }
		[JsonProperty("typeId")]
		public string NoteTypeId { get; set; }
		public NoteType Type { get; set; }
		[JsonProperty("addition_content")]
		public Dictionary<string, byte[]> Images { get; set; }
		[JsonProperty("checklist")]
		public List<ChecklistItem> Checklist { get; set; }
		[JsonProperty("state")]
		public NoteState State { get; set; }
		[JsonProperty("last_time_modifired")]
		public DateTime LastEdited { get; set; }
		[JsonProperty("notification_time")]
		public DateTime? NotificationTime { get; set; }
		[JsonProperty("notificationId")]
		public int NotificationId { get; set; }
	}
}
