using NotesAndReminders.Models;
using System.Threading.Tasks;

namespace NotesAndReminders.Services
{
	public interface IAuthorizationService
	{
		bool IsLoggedIn { get; }
		Task<(User, string)> LogIn(string email, string password);

		Task<(User, string)> SingUp(string email, string password);


		bool LogOut();


	}
}
