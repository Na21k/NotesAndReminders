using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class MyNotesView : ContentPage
	{
		public MyNotesView()
		{
			InitializeComponent();

			BindingContext = new MyNotesViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as MyNotesViewModel;
			vm?.OnAppearing();
		}
	}
}
