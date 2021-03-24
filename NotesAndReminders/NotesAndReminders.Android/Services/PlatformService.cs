using NotesAndReminders.Droid.Services;
using NotesAndReminders.Services;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformService))]
namespace NotesAndReminders.Droid.Services
{
	public class PlatformService : IPlatformService
	{
		public void SetStatusbarColor(string hexCode)
		{
			var currentActivity = Platform.CurrentActivity;
			var color = Android.Graphics.Color.ParseColor(hexCode);
			currentActivity?.Window?.SetStatusBarColor(color);
		}

		public void SetNavbarColor(string hexCode)
		{
			var currentActivity = Platform.CurrentActivity;
			var color = Android.Graphics.Color.ParseColor(hexCode);
			currentActivity?.Window?.SetNavigationBarColor(color);
		}

		public void SetTranslucentStatusbar()
		{
			var currentActivity = Platform.CurrentActivity;
			currentActivity.Window.AddFlags(Android.Views.WindowManagerFlags.TranslucentStatus);
		}

		public void SetTranslucentNavbar()
		{
			var currentActivity = Platform.CurrentActivity;
			currentActivity.Window.AddFlags(Android.Views.WindowManagerFlags.TranslucentNavigation);
		}
	}
}
