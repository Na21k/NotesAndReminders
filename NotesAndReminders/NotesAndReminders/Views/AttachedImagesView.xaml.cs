using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class AttachedImagesView : ContentPage
	{
		public AttachedImagesView()
		{
			InitializeComponent();

			BindingContext = new AttachedImagesViewModel();
		}
	}
}
