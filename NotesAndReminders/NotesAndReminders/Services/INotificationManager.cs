using System;
using System.Collections.Generic;
using System.Text;

namespace NotesAndReminders.Services
{
	public interface INotificationManager
	{
		event EventHandler NotificationReceived;
		void Initialize();
		void SendNotification(string title, string message, int id, DateTime? notifyTime = null);
		void ReceiveNotification(string title, string message);
		void Cancel(int id);
	}
}
