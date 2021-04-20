using NotesAndReminders.Resources;
using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class NewCategoryView : ContentPage
	{
		public NewCategoryView()
		{
			InitializeComponent();

			BindingContext = new NewCategoryViewModel();

			MessagingCenter.Subscribe<NewCategoryViewModel>(this, Constants.UnexpectedErrorEvent, OnUnexpectedErrorEvent);
		}

		private async void OnUnexpectedErrorEvent(NewCategoryViewModel vm)
		{
			await DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
		}
	}
}
