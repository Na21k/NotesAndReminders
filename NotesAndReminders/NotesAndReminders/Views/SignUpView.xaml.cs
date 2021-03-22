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

			BindingContext = new SignUpViewModel();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Delay(500);
			emailEntry.Focus();
		}
	}
}
