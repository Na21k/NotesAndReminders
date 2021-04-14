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

		protected async override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (propertyName.Equals("CurrentItem") && Device.RuntimePlatform == Device.Android) //workaround to avoid menu lags
			{
				FlyoutIsPresented = false;
				await Task.Delay(500);
			}
			else if (propertyName.Equals("FlyoutIsPresented"))
			{
				OnPropertyChanged(nameof(LogoText));    //update LogoText if user does not restart app for a long period of time
			}

			base.OnPropertyChanged(propertyName);
		}

		/*private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Current.GoToAsync("//LoginPage", true);
		}*/
	}
}
