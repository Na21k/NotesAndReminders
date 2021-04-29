
using NotesAndReminders.Converters;

namespace NotesAndReminders.Models
{
	public class NoteType : Identifiable, IDBItem
	{
		[Newtonsoft.Json.JsonProperty("name")]
		public string Name { get; set; }
		[Newtonsoft.Json.JsonProperty("noteColor")]
		public NoteColorModel Color { get; set; }
	}
}
