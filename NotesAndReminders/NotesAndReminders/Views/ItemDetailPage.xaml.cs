using NotesAndReminders.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}