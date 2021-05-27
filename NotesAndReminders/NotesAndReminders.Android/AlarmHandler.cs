using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NotesAndReminders.Droid.Services.NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace NotesAndReminders.Droid
{
	[BroadcastReceiver(Enabled = true, Label = "Local Notification Broadcast Receiver")]
	public class AlarmHandler : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			if(intent?.Extras != null)
			{
				string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
				string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);

				AndroidNotificationManager manager = AndroidNotificationManager.Instance ?? new AndroidNotificationManager();
				manager.Show(title, message);
			}
		}
	}
}