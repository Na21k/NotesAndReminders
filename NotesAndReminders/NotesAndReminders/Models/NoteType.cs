using Newtonsoft.Json;

namespace NotesAndReminders.Models
{
	public class NoteType : Identifiable, IDBItem
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("noteColor")]
		public NoteColorModel Color { get; set; }
	}
}
