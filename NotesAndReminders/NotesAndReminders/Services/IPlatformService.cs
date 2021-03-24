namespace NotesAndReminders.Services
{
	public interface IPlatformService
	{
		void SetStatusbarColor(string hexCode);
		void SetNavbarColor(string hexCode);
		void SetTranslucentStatusbar();
		void SetTranslucentNavbar();
	}
}
