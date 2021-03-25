using NotesAndReminders.Resources;
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
			MessagingCenter.Subscribe<LogInViewModel>(this, Constants.EmptyLoginOrPasswordEvent, (logInViewModel) => OnEmptyLoginOrPassword());

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
			await DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
		}

		private async void OnLoggedInAsync()
		{
			await Shell.Current.Navigation.PopToRootAsync();
		}

		private async void OnInvalidLoginOrPasswordAsync()
		{
			await DisplayAlert(AppResources.Oops, AppResources.InvalidLoginOrPassword, AppResources.Ok);
		}

		private async void OnEmptyLoginOrPassword()
		{
			await DisplayAlert(AppResources.Error, AppResources.EmptyLoginOrPassword, AppResources.Ok);
		}
	}
}
