using Xamarin.Essentials;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class AboutAppView : ContentPage
	{
		public string Version => VersionTracking.CurrentVersion;
		public string Build => VersionTracking.CurrentBuild;

		public AboutAppView()
		{
			InitializeComponent();

			BindingContext = this;
		}
	}
}
