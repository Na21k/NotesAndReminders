using Xamarin.Forms;

namespace NotesAndReminders.Models
{
	public class NoteType : Identifiable, IDBItem
	{
		public string Name { get; set; }
		public Color Color { get; set; }
	}
}
