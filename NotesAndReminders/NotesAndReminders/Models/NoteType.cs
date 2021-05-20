using Newtonsoft.Json;
using NotesAndReminders.Converters;
using Xamarin.Forms;

namespace NotesAndReminders.Models
{
	public class NoteType : Identifiable, IDBItem
	{
		[JsonProperty("noteColorDark")]
		[JsonConverter(typeof(ColorJsonConverter))]
		public Color DarkTheme { get; set; }
		[JsonProperty("noteColorLight")]
		[JsonConverter(typeof(ColorJsonConverter))]
		public Color LightTheme { get; set; }
		[JsonProperty("name")]
		public string Name { get; set; }
		public NoteColorModel Color { get; set; }

		public NoteType()
		{
			Color = new NoteColorModel
			{
				Dark = DarkTheme,
				Light = LightTheme
			};
		}
	}
}
