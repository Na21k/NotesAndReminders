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

			MessagingCenter.Subscribe<TrashViewModel>(this, Constants.HideEmptyTrashButton, RemoveEmptyTrashButton);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var vm = BindingContext as TrashViewModel;
			vm?.OnAppearing();
		}

		private void RemoveEmptyTrashButton(TrashViewModel vm)
		{
			ToolbarItems.Remove(emptyTrashToolbarItem);
		}
	}
}
