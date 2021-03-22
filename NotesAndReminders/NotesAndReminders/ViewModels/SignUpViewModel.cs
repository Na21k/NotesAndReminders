using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class SignUpViewModel : BaseViewModel
	{
		private string _email;
		private string _password;
		private string _confirmPassword;

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

		public ICommand SignUpCommand { get; private set; }

		public SignUpViewModel()
		{
			SignUpCommand = new Command(SignUpAsync);
		}

		private async void SignUpAsync()
		{

		}
	}
}
