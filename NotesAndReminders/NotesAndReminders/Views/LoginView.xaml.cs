using NotesAndReminders.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class LogInView : ContentPage
	{
		public LogInView()
		{
			InitializeComponent();

			MessagingCenter.Subscribe<LogInViewModel>(this, Constants.UnexpectedErrorEvent, (logInViewModel) => OnUnexpectedErrorAsync());
			MessagingCenter.Subscribe<LogInViewModel>(this, Constants.LoggedInEvent, (logInViewModel) => OnLoggedInAsync());
			MessagingCenter.Subscribe<LogInViewModel>(this, Constants.InvalidLoginOrPasswordEvent, (logInViewModel) => OnInvalidLoginOrPasswordAsync());

			BindingContext = new LogInViewModel();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Delay(500);
			loginEntry.Focus();
		}

		private async void OnUnexpectedErrorAsync()
		{
			await DisplayAlert("Error", "Unexpected error has occurred", "OK");
		}

		private async void OnLoggedInAsync()
		{
			await Shell.Current.Navigation.PopToRootAsync();
		}

		private async void OnInvalidLoginOrPasswordAsync()
		{
			await DisplayAlert("Oops", "Invalid login or password", "OK");
		}
	}
}
