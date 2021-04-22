using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class ArchivedNotesView : ContentPage
	{
		public ArchivedNotesView()
		{
			InitializeComponent();

			BindingContext = new ArchivedNotesViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as ArchivedNotesViewModel;
			vm?.OnAppearing();
		}
	}
}
