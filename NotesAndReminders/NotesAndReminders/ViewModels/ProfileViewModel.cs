using NotesAndReminders.Models;
using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class ProfileViewModel : BaseViewModel
	{
		private IAuthorizationService _authorizationService;

		public bool IsLoggedIn => _authorizationService.IsLoggedIn;
		public User User => Settings.User;

		public ICommand LogInCommand { get; private set; }
		public ICommand SignUpCommand { get; private set; }
		public ICommand LogOutCommand { get; private set; }

		public ProfileViewModel()
		{
			_authorizationService = DependencyService.Get<IAuthorizationService>();

			LogInCommand = new Command(LogInAsync);
			SignUpCommand = new Command(SignUpAsync);
			LogOutCommand = new Command(LogOut);

			MessagingCenter.Subscribe<LogInViewModel>(this, Constants.LoggedInEvent, (vm) => OnLoggedIn());
			MessagingCenter.Subscribe<SignUpViewModel>(this, Constants.LoggedInEvent, (vm) => OnLoggedIn());
		}

		private async void LogInAsync()
		{
			await Shell.Current.GoToAsync(nameof(LogInView));
		}

		private async void SignUpAsync()
		{
			await Shell.Current.GoToAsync(nameof(SignUpView));
		}

		private void LogOut()
		{
			if (_authorizationService.LogOut())
			{
				Settings.User = null;
				Settings.UserToken = null;

				OnPropertyChanged(nameof(IsLoggedIn));
				OnPropertyChanged(nameof(User));
			}
			else
			{
				MessagingCenter.Send(this, Constants.UnexpectedErrorEvent);
			}
		}

		private void OnLoggedIn()
		{
			OnPropertyChanged(nameof(IsLoggedIn));
			OnPropertyChanged(nameof(User));
		}
	}
}
