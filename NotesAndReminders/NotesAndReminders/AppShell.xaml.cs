using NotesAndReminders.Resources;
using NotesAndReminders.Views;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders
{
	public partial class AppShell : Shell
	{
		public string LogoText
		{
			get
			{
				var time = DateTime.Now.TimeOfDay;

				if (time < new TimeSpan(5, 0, 0))
				{
					return AppResources.LogoTextGoodNight;
				}
				else if (time < new TimeSpan(11, 0, 0))
				{
					return AppResources.LogoTextGoodMorning;
				}
				else if (time < new TimeSpan(14, 0, 0))
				{
					return AppResources.LogoTextGoodAfternoon;
				}
				else if (time < new TimeSpan(21, 0, 0))
				{
					return AppResources.LogoTextGoodDay;
				}
				else
				{
					return AppResources.LogoTextGoodEvening;
				}
			}
		}

		public AppShell()
		{
			InitializeComponent();

			BindingContext = this;

			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

			Routing.RegisterRoute(nameof(LogInView), typeof(LogInView));
			Routing.RegisterRoute(nameof(SignUpView), typeof(SignUpView));
			Routing.RegisterRoute(nameof(NoteDetailsView), typeof(NoteDetailsView));
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

		protected override void OnAppearing()
		{
			base.OnAppearing();

			OnPropertyChanged(nameof(LogoText));
		}

		/*private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Current.GoToAsync("//LoginPage", true);
		}*/
	}
}
