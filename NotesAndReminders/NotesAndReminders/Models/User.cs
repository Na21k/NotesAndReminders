namespace NotesAndReminders.Models
{
	public class User : Identifiable, IDBItem
	{
		public string UserName { get; set; }
		public string Email { get; set; }
	}
}
