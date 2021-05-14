using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class TrashView : ContentPage
	{
		public TrashView()
		{
			InitializeComponent();

			BindingContext = new TrashViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as TrashViewModel;
			vm?.OnAppearing();
		}
	}
}
