using NotesAndReminders.Resources;
using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class ProfileView : ContentPage
	{
		public ProfileView()
		{
			InitializeComponent();

			MessagingCenter.Subscribe<ProfileViewModel>(this, Constants.UnexpectedErrorEvent, (profileViewModel) => OnUnexpectedError());

			BindingContext = new ProfileViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			var vm = BindingContext as BaseViewModel;
			vm?.OnAppearing();
		}

		private async void OnUnexpectedError()
		{
			await DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
		}
	}
}
