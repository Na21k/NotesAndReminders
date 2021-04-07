using Android.Gms.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using NotesAndReminders.Droid.Services;
using NotesAndReminders.Exceptions;
using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthService))]
namespace NotesAndReminders.Droid.Services
{
	public class FirebaseAuthService : IAuthorizationService
	{
		private FirebaseAuth _auth = FirebaseAuth.Instance;

		public bool IsLoggedIn
		{
			get
			{
				var user = _auth.CurrentUser;
				return user != null;
			}
		}

		public async Task<(User, string)> LogIn(string email, string password)
		{
			try
			{
				var user = await _auth.SignInWithEmailAndPasswordAsync(email, password);
				var token = await user.User.GetIdToken(false) as GetTokenResult;

				var userModel = new User()
				{
					Email = user.User.Email,
					UserName = user.User.DisplayName,
					Id = _auth.CurrentUser.Uid
				};

				if (token == null)
				{
					throw new NullReferenceException($"{typeof(FirebaseAuthService)}: {nameof(token)} was null");
				}

				return (userModel, token?.Token);
			}
			catch (FirebaseAuthInvalidUserException ex)
			{
				ex.PrintStackTrace();
				throw new Exception(ex.Message);
			}
			catch (FirebaseAuthInvalidCredentialsException ex)
			{
				ex.PrintStackTrace();
				throw new InvalidCredentialsException(ex.Message, ex.StackTrace);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.StackTrace);
				throw new Exception(ex.Message);
			}
		}

		public bool LogOut()
		{
			try
			{
				_auth.SignOut();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<(User, string)> SignUp(string email, string password)
		{
			try
			{
				var user = await _auth.CreateUserWithEmailAndPasswordAsync(email, password);
				var res = await LogIn(email, password);

				return res;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<bool> SendEmailVerification()
		{
			var user = _auth.CurrentUser;
			using (var actionCode = ActionCodeSettings.NewBuilder().SetAndroidPackageName("Test", true, "0").Build())
			{
				await user.SendEmailVerification(actionCode);
			}

			return user.IsEmailVerified;
		}

		public async Task<bool> ResetPassword(string email)
		{
			try
			{
				await _auth.SendPasswordResetEmailAsync(email);

				return true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public Task<User> GetUserAsync(Action<IDBItem> onUserRecievedCallback)
		{
			var tcs = new TaskCompletionSource<User>();

			//FirebaseFirestore.Instance
			//	.Collection("Users")
			//	.Document(_auth.CurrentUser.Uid)
			//	.Get()
			//	.AddOnCompleteListener(new OnUserCompleteListener<User>(onUserRecievedCallback));
			 
			return tcs.Task;
		}
	}
}
