using NotesAndReminders.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class SignUpViewModel : BaseViewModel
	{
		private IAuthorizationService _authorizationService;

		private string _email;
		private string _password;
		private string _confirmPassword;
		private bool _isSigningUp;

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
		public string ConfirmPassword
		{
			get => _confirmPassword;
			set => SetProperty(ref _confirmPassword, value);
		}
		public bool IsSigningUp
		{
			get => _isSigningUp;
			set => SetProperty(ref _isSigningUp, value);
		}

		public ICommand SignUpCommand { get; private set; }

		public SignUpViewModel()
		{
			_authorizationService = DependencyService.Get<IAuthorizationService>();
			SignUpCommand = new Command(SignUpAsync);
		}

		private async void SignUpAsync()
		{
			if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
			{
				MessagingCenter.Send(this, Constants.EmptyLoginOrPasswordEvent);
				return;
			}
			else if (!Password.Equals(ConfirmPassword))
			{
				MessagingCenter.Send(this, Constants.PasswordFieldDoesNotMatchConfirmPasswordFieldEvent);
				return;
			}

			try
			{
				IsSigningUp = true;

				var res = await _authorizationService.SignUp(Email, Password);

				Settings.User = res.Item1;
				Settings.UserToken = res.Item2;

				MessagingCenter.Send(this, Constants.LoggedInEvent);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);

				MessagingCenter.Send(this, Constants.UnexpectedErrorEvent);
			}
			finally
			{
				IsSigningUp = false;
			}
		}
	}
}
