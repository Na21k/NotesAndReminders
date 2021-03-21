using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class ProfileView : ContentPage
	{
		public ProfileView()
		{
			InitializeComponent();

			BindingContext = new ProfileViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			var vm = BindingContext as BaseViewModel;
			vm?.OnAppearing();
		}
	}
}
