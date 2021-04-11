using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class NoteDetailsView : ContentPage
	{
		public NoteDetailsView()
		{
			InitializeComponent();

			BindingContext = new NoteDetailsViewModel();
		}
	}
}
