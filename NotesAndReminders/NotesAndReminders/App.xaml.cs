using NotesAndReminders.Services;
using Xamarin.Forms;

namespace NotesAndReminders
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();

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
