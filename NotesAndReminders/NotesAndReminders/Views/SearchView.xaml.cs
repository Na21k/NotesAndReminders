using NotesAndReminders.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class SearchView : ContentPage
	{
		public SearchView()
		{
			InitializeComponent();

			BindingContext = new SearchViewModel();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Delay(500);
			searchEntry.Focus();
		}
	}
}
