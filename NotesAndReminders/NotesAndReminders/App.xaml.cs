using NotesAndReminders.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NotesAndReminders
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			VersionTracking.Track();

			var platformService = DependencyService.Get<IPlatformService>();
			platformService.SetTranslucentStatusbar();
			platformService.SetTranslucentNavbar();

			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
