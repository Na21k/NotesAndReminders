using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NotesAndReminders.Droid.Services;
using NotesAndReminders.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseAuthService))]
namespace NotesAndReminders.Droid.Services
{
	public class FirebaseAuthService : IAuthorizationService
	{

	}
}
