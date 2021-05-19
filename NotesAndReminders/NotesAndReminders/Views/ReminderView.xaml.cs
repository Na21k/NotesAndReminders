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
		}
	}
}
