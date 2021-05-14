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

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.HideSaveAndDeleteButtonsEvent, HideSaveAndDeleteButtons);
		}

		public void HideSaveAndDeleteButtons(NoteDetailsViewModel vm)
		{
			ToolbarItems.Remove(deleteBtn);
			ToolbarItems.Remove(saveBtn);
			mainStack.IsEnabled = false;
			newChecklistItemStack.IsVisible = false;
		}
	}
}
