using Android.App;
using Android.Content;
using Android.Graphics;
using AndroidX.Core.App;
using NotesAndReminders.Droid.Services.NotificationService;
using NotesAndReminders.Models;
using System.IO;
using System.Xml.Serialization;
using AndroidApp = Android.App.Application;

namespace NotesAndReminders.Droid
{
	[BroadcastReceiver(Enabled = true, Process =":remote", Label = "Local Notification Broadcast Receiver")]
	public class AlarmHandler : BroadcastReceiver
	{
		public const string LocalNotificationKey = "LocalNotification";
		public override void OnReceive(Context context, Intent intent)
		{
			if (intent?.Extras != null)
			{
				string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
				string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);

				AndroidNotificationManager manager = AndroidNotificationManager.Instance ?? new AndroidNotificationManager();
				manager.Show(title, message);
			}
		}
	}
}
