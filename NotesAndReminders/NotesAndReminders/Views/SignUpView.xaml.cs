using NotesAndReminders.Resources;
using NotesAndReminders.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class SignUpView : ContentPage
	{
		public SignUpView()
		{
			InitializeComponent();

			MessagingCenter.Subscribe<SignUpViewModel>(this, Constants.EmptyLoginOrPasswordEvent,
				(signUpViewModel) => OnEmptyLoginOrPasswordAsync());
			MessagingCenter.Subscribe<SignUpViewModel>(this, Constants.PasswordFieldDoesNotMatchConfirmPasswordFieldEvent,
				(signUpViewModel) => OnPasswordFieldDoesNotMatchConfirmPasswordFieldAsync());
			MessagingCenter.Subscribe<SignUpViewModel>(this, Constants.LoggedInEvent, (signUpViewModel) => OnLoggedInAsync());
			MessagingCenter.Subscribe<SignUpViewModel>(this, Constants.UnexpectedErrorEvent,
				(signUpViewModel) => OnUnexpectedErrorAsync());

			BindingContext = new SignUpViewModel();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Delay(500);
			emailEntry.Focus();
		}

		private async void OnEmptyLoginOrPasswordAsync()
		{
			await DisplayAlert(AppResources.Error, AppResources.EmptyLoginOrPassword, AppResources.Ok);
		}

		private async void OnPasswordFieldDoesNotMatchConfirmPasswordFieldAsync()
		{
			await DisplayAlert(AppResources.Error, AppResources.PasswordFieldDoesNotMatchConfirmPasswordField, AppResources.Ok);
		}

		private async void OnLoggedInAsync()
		{
			await Shell.Current.Navigation.PopToRootAsync();
		}

		private async void OnUnexpectedErrorAsync()
		{
			await DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
		}
	}
}
