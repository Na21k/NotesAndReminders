using NotesAndReminders.Resources;
using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class NotesCategoriesView : ContentPage
	{
		public NotesCategoriesView()
		{
			InitializeComponent();

			BindingContext = new NotesCategoriesViewModel();

			MessagingCenter.Subscribe<NotesCategoriesViewModel>(this, Constants.UnexpectedErrorEvent, OnUnexpectedErrorEvent);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as NotesCategoriesViewModel;
			vm?.OnAppearing();
		}

		private async void OnUnexpectedErrorEvent(NotesCategoriesViewModel vm)
		{
			await DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
		}
	}
}
