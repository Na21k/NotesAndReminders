using NotesAndReminders.Models;
using NotesAndReminders.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class ProfileViewModel : BaseViewModel
	{
		public bool IsLoggedIn => User != null;
		public User User => Settings.User;

		public ICommand LogIn { get; private set; }
		public ICommand SignUp { get; private set; }

		public ProfileViewModel()
		{
			LogIn = new Command(LogInAsync);
			SignUp = new Command(SignUpAsync);
		}

		private async void LogInAsync()
		{
			await Shell.Current.GoToAsync(nameof(LogInView));
		}

		private async void SignUpAsync()
		{

		}
	}
}
