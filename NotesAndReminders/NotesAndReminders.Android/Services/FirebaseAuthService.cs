using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NotesAndReminders.Droid.Services;
using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthService))]
namespace NotesAndReminders.Droid.Services
{
	public class FirebaseAuthService : IAuthorizationService
	{
		public bool IsLoggedIn => throw new NotImplementedException();

		public Task<(User, string)> LogIn(string email, string password)
		{
			throw new NotImplementedException();
		}

		public bool LogOut()
		{
			throw new NotImplementedException();
		}
	}
}
