using NotesAndReminders.ViewModels;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class ReminderView : ContentPage
	{
		public ReminderView()
		{
			InitializeComponent();

			BindingContext = new ReminderViewModel();

			MessagingCenter.Subscribe<ReminderViewModel>(this, Constants.HideDeleteReminderButton, HideDeleteButton);
		}

		public void HideDeleteButton(ReminderViewModel vm)
		{
			ToolbarItems.Remove(deleteReminderBtn);
		}
	}
}
