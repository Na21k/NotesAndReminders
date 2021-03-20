using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotesAndReminders
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
