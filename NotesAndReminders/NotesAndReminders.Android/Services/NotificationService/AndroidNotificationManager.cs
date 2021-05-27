using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;
using System.Xml.Serialization;
using System.IO;

[assembly: Dependency(typeof(NotesAndReminders.Droid.Services.NotificationService.AndroidNotificationManager))]
namespace NotesAndReminders.Droid.Services.NotificationService
{
	public class AndroidNotificationManager : INotificationManager
	{
		const string channelId = "default";
		const string channekName = "Default";
		const string channelDescription = "The default channel for notifications";

		public const string TitleKey = "title";
		public const string MessageKey = "message";

		bool channelInitialized = false;
		int messageId = 0;
		int pendingIntentId = 0;

		NotificationManager notificationManager;

		public event EventHandler NotificationReceived;

		public static AndroidNotificationManager Instance { get; private set; }

		public AndroidNotificationManager() => Initialize();

		public void Initialize()
		{
			if(Instance == null)
			{
				CreateNotificationChannel();
				Instance = this;
			}
		}
		private void CreateNotificationChannel()
		{
			notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
			{
				var channelNameJava = new Java.Lang.String(channekName);
				var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
				{
					Description = channelDescription
				};
				notificationManager.CreateNotificationChannel(channel);
			}

			channelInitialized = true;
		}

		public void SendNotification(string title, string message,int id, DateTime? notifyTime = null)
		{
			if (!channelInitialized)
			{
				CreateNotificationChannel();
			}

			if(notifyTime != null)
			{
				var localNotification = new LocalNotification();
				localNotification.Title = title;
				localNotification.Body = message;
				localNotification.Id = id;

				var serialzedNoti = SerializeNotification(localNotification);

				var intent = CreateIntent(id);
				intent.PutExtra(AlarmHandler.LocalNotificationKey,serialzedNoti);

				PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
				long triggerTime = GetNotifyTime(notifyTime.Value);
				AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
				alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
			}
			else
			{
				Show(title, message);
			}
		}
		private string SerializeNotification(LocalNotification notification)
		{
			var xmlSerializer = new XmlSerializer(notification.GetType());
			using (var stringWriter = new StringWriter())
			{
				xmlSerializer.Serialize(stringWriter, notification);
				return stringWriter.ToString();
			}
		}

		private long GetNotifyTime(DateTime value)
		{
			DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(value);
			double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
			long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
			return utcAlarmTime;
		}
		public static Intent GetLauncherActivity()
		{
			var packageName = AndroidApp.Context.PackageName;
			return AndroidApp.Context.PackageManager.GetLaunchIntentForPackage(packageName);
		}

		public void ReceiveNotification(string title, string message)
		{
			var args = new NotificationEventArgs()
			{
				Title = title,
				Message = message,
			};
			NotificationReceived?.Invoke(null, args);
		}

		public void Show(string title, string message)
		{
			Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
			intent.PutExtra(TitleKey, title);
			intent.PutExtra(MessageKey, message);

			PendingIntent pendingIntent = PendingIntent.GetActivities(AndroidApp.Context, pendingIntentId++, (Intent[])intent, PendingIntentFlags.UpdateCurrent);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
				.SetContentIntent(pendingIntent)
				.SetContentTitle(title)
				.SetContentText(message)
				.SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.logo_large))
				.SetSmallIcon(Resource.Drawable.logo)
				.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate | (int)NotificationDefaults.Lights);

			Notification notification = builder.Build();
			notificationManager.Notify(messageId++, notification);

		}

		private Intent CreateIntent(int id)
		{
			return new Intent(AndroidApp.Context, typeof(AlarmHandler))
				.SetAction("LocalNotifierIntent" + id);
		}
		public void Cancel(int id)
		{
			var intent = CreateIntent(id);
			var pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, 0, intent, PendingIntentFlags.CancelCurrent);

			var alarmManager = GetAlarmManager();
			alarmManager.Cancel(pendingIntent);

			var notificationManager = NotificationManagerCompat.From(AndroidApp.Context);
			notificationManager.Cancel(id);
		}

		private AlarmManager GetAlarmManager()
		{
			var alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
			return alarmManager;
		}

	}
}