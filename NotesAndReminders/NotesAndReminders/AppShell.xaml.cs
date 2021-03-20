using NotesAndReminders.Views;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
		}

		//workaround to avoid menu lags
		protected async override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (propertyName.Equals("CurrentItem") && Device.RuntimePlatform == Device.Android)
			{
				FlyoutIsPresented = false;
				await Task.Delay(500);
			}

			base.OnPropertyChanged(propertyName);
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Current.GoToAsync("//LoginPage", true);
		}
	}
}
