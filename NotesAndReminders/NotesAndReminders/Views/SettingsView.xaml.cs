using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class SettingsView : ContentPage
	{
		public SettingsView()
		{
			InitializeComponent();

			BindingContext = new SettingsViewModel();
		}
	}
}
