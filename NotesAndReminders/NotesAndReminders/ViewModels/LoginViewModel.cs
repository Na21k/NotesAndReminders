using NotesAndReminders.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class LogInViewModel : BaseViewModel
	{
		private IAuthorizationService _authorizationService;

		private string _email;
		private string _password;

		public string Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}
		public string Password
		{
			get => _password;
			set => SetProperty(ref _password, value);
		}

		public ICommand LogInCommand { get; private set; }

		public LogInViewModel()
		{
			_authorizationService = DependencyService.Get<IAuthorizationService>();
			LogInCommand = new Command(LogInAsync);
		}

		private async void LogInAsync()
		{

		}
	}
}
