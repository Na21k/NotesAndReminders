﻿using NotesAndReminders.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotesAndReminders.Views
{
	public partial class LogInView : ContentPage
	{
		public LogInView()
		{
			InitializeComponent();

			BindingContext = new LogInViewModel();
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Delay(500);
			loginEntry.Focus();
		}
	}
}