using NotesAndReminders.Exceptions;
using NotesAndReminders.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class LogInViewModel : BaseViewModel
	{
		private IAuthorizationService _authorizationService;

		private string _email;
		private string _password;
		private bool _isLoggingIn;

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
		public bool IsLoggingIn
		{
			get => _isLoggingIn;
			set => SetProperty(ref _isLoggingIn, value);
		}

		public ICommand LogInCommand { get; private set; }

		public LogInViewModel()
		{
			_authorizationService = DependencyService.Get<IAuthorizationService>();
			LogInCommand = new Command(LogInAsync);
		}

		private async void LogInAsync()
		{
			try
			{
				IsLoggingIn = true;
				var res = await _authorizationService.LogIn(Email, Password);

				Settings.User = res.Item1;
				Settings.UserToken = res.Item2;

				MessagingCenter.Send(this, Constants.LoggedInEvent);
			}
			catch (InvalidCredentialsException ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				MessagingCenter.Send(this, Constants.InvalidLoginOrPasswordEvent);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				MessagingCenter.Send(this, Constants.UnexpectedErrorEvent);
			}
			finally
			{
				IsLoggingIn = false;
			}
		}
	}
}
