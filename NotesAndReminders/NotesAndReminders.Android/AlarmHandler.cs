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
	[BroadcastReceiver(Enabled = true, Label = "Local Notification Broadcast Receiver")]
	public class AlarmHandler : BroadcastReceiver
	{
		public const string LocalNotificationKey = "LocalNotification";


		public override void OnReceive(Context context, Intent intent)
		{
			if(intent?.Extras != null)
			{

				var extra = intent.GetStringExtra(LocalNotificationKey);
				var notification = DeserializeNotification(extra);

				var builder = new NotificationCompat.Builder(AndroidApp.Context)
					.SetContentTitle(notification.Title)
					.SetContentText(notification.Body)
					.SetSmallIcon(notification.IconId)
					.SetAutoCancel(true)
					.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.logo_large))
					.SetSmallIcon(Resource.Drawable.logo)
					.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate | (int)NotificationDefaults.Lights);

				var resultIntent = AndroidNotificationManager.GetLauncherActivity();
				resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
				var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(AndroidApp.Context);
				stackBuilder.AddNextIntent(resultIntent);
				var resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
				builder.SetContentIntent(resultPendingIntent);

				var notificationManager = NotificationManagerCompat.From(AndroidApp.Context);
				notificationManager.Notify(notification.Id, builder.Build());

			}
		}
		private LocalNotification DeserializeNotification(string notificationString)
		{
			var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
			using (var stringReader = new StringReader(notificationString))
			{
				var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
				return notification;
			}
		}
	}
}