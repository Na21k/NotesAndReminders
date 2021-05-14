using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class NewOrEditCategoryView : ContentPage
	{
		public NewOrEditCategoryView()
		{
			InitializeComponent();

			BindingContext = new NewOrEditCategoryViewModel();
		}
	}
}
