using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Icu.Util;
using Android.OS;
using AndroidX.Core.App;
using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

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
			if (Instance == null)
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
		public void SendNotification(string title, string message, int id, DateTime? notifyTime = null)
		{
			if (!channelInitialized)
			{
				CreateNotificationChannel();
			}

			if (notifyTime != null)
			{
				Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
				intent.PutExtra(TitleKey, title);
				intent.PutExtra(MessageKey, message);

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

			PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

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


		long GetNotifyTime(DateTime notifyTime)
		{

			DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan elapsedTime = notifyTime - epoch;

			var time = elapsedTime.TotalMilliseconds;
			return (long)time;
		}


		public void DeleteNotification(int id)
		{
			Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
			PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, id, intent, PendingIntentFlags.CancelCurrent);
			AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
			alarmManager.Cancel(pendingIntent);

			var notificationManager = NotificationManagerCompat.From(AndroidApp.Context);
			notificationManager.Cancel(id);
		}

	}
}
