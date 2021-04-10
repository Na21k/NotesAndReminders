using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Firebase;
using Firebase.Firestore;

namespace NotesAndReminders.Droid
{
	[Activity(Label = "NotesAndReminders", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			var options = new FirebaseOptions.Builder()
				.SetProjectId("notesandreminders-5c2a6")
				.SetApplicationId("1:289508089037:android:01bcec6a66578b5e125d84")
				.SetApiKey("AIzaSyBXueYS_t18Nxd_F-DsYtLdEdFnmypJFjY")
				.SetDatabaseUrl("https://notesandreminders-5c2a6.firebaseio.com")
				.SetStorageBucket("notesandreminders-5c2a6.appspot.com")
				.SetGcmSenderId("289508089037")
				.Build();

			FirebaseApp.InitializeApp(Application.Context);
			
			LoadApplication(new App());
		}


		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}