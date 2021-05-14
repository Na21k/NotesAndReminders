using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class ProfileViewModel : BaseViewModel
	{
		private IAuthorizationService _authorizationService;
		private TrashService _trashService;

		public bool IsLoggedIn => _authorizationService.IsLoggedIn;
		public User User => Settings.User;

		public ICommand LogInCommand { get; private set; }
		public ICommand SignUpCommand { get; private set; }
		public ICommand LogOutCommand { get; private set; }

		public ProfileViewModel()
		{
			_authorizationService = DependencyService.Get<IAuthorizationService>();
			_trashService = new TrashService();

			LogInCommand = new Command(LogInAsync);
			SignUpCommand = new Command(SignUpAsync);
			LogOutCommand = new Command(LogOutAsync);

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

		private async void LogOutAsync()
		{
			if (await Shell.Current.DisplayAlert($"{AppResources.Continue}?", AppResources.YouWillLoseYourTrashContents, AppResources.Ok, AppResources.Cancel))
			{
				if (_authorizationService.LogOut())
				{
					Settings.User = null;
					Settings.UserToken = null;

					OnPropertyChanged(nameof(IsLoggedIn));
					OnPropertyChanged(nameof(User));

					_trashService.EmptyTrash();
					MessagingCenter.Send(this, Constants.LoggedOutEvent);
				}
				else
				{
					MessagingCenter.Send(this, Constants.UnexpectedErrorEvent);
				}
			}
		}

		private void OnLoggedIn()
		{
			OnPropertyChanged(nameof(IsLoggedIn));
			OnPropertyChanged(nameof(User));
		}
	}
}
