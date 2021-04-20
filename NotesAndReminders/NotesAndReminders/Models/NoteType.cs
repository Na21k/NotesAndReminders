using Newtonsoft.Json;
using NotesAndReminders.Converters;

namespace NotesAndReminders.Models
{
	public class NoteType : Identifiable, IDBItem
	{
		public string Name { get; set; }
		[JsonConverter(typeof(ColorJsonConverter))]
		public NoteColorModel Color { get; set; }
	}
}
