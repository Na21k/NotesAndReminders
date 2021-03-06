using NotesAndReminders.Models;
using System;
using System.Threading.Tasks;

namespace NotesAndReminders.Services
{
	public interface IAuthorizationService
	{
		bool IsLoggedIn { get; }
		Task<(User, string)> LogIn(string email, string password);
		Task<(User, string)> SignUp(string email, string password);
		Task<bool> ResetPassword(string email);
		bool LogOut();

		Task<User> GetUserAsync(Action<IDBItem> onUserRecievedCallback);
	}
}
