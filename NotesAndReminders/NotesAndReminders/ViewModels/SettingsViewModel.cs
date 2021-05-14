using NotesAndReminders.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class SettingsViewModel : BaseViewModel
	{
		public ICommand AboutAppCommand { get; private set; }

		public SettingsViewModel() : base()
		{
			AboutAppCommand = new Command(AboutAppAsync);
		}

		private async void AboutAppAsync()
		{
			await Shell.Current.GoToAsync(nameof(AboutAppView));
		}
	}
}
