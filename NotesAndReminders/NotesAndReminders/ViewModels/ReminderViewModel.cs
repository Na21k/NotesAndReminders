using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class ReminderViewModel : BaseViewModel
	{
		private IDBService _dBService;
		private Note _note;
		private TimeSpan _time;
		private DateTime _date;
		private bool _isSaving;

		public bool IsSaving
		{
			get => _isSaving;
			set
			{
				SetProperty(ref _isSaving, value);
			}
		}
		public TimeSpan Time
		{
			get => _time;
			set => SetProperty(ref _time, value);
		}
		public DateTime Date
		{
			get => _date;
			set => SetProperty(ref _date, value);
		}
		public DateTime MinimumDate => DateTime.Now.Date;

		public ICommand SaveCommand { get; private set; }
		public ICommand DeleteReminderCommand { get; private set; }

		public ReminderViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();

			Time = DateTime.Now.AddHours(1).TimeOfDay;
			Date = DateTime.Now.AddDays(1).Date;

			SaveCommand = new Command(SaveAsync);
			DeleteReminderCommand = new Command(DeleteReminderAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.ManageNoteReminderOpened, Init);
		}

		private void Init(NoteDetailsViewModel vm)
		{
			_note = vm.Note;

			if (_note.NotificationTime.HasValue)
			{
				var noteNotificationTime = _note.NotificationTime.Value.TimeOfDay;
				var noteNotificationDate = _note.NotificationTime.Value.Date;

				Time = noteNotificationTime;
				Date = noteNotificationDate;
			}
			else
			{
				MessagingCenter.Send(this, Constants.HideDeleteReminderButton);
			}
		}

		private async void SaveAsync()
		{
			if (_note == null)
			{
				return;
			}

			var selectedValue = new DateTime(Date.Ticks + Time.Ticks);

			if (selectedValue <= DateTime.Now)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.ReminderTimeCanNotBeLessThanCurrent, AppResources.Ok);

				return;
			}

			IsSaving = true;
			_note.NotificationTime = selectedValue;
			var notificationId = _dBService.CreateNotification(_note);
			_note.NotificationId = notificationId;

			await Shell.Current.Navigation.PopAsync();
		}

		private async void DeleteReminderAsync()
		{
			if (_note == null)
			{
				return;
			}

			if (await Shell.Current.DisplayAlert(null, AppResources.DeleteReminderForThisNoteQuestion, AppResources.Ok, AppResources.Cancel))
			{
				IsSaving = true;
				_note.NotificationTime = null;

				await Shell.Current.Navigation.PopAsync();
			}
		}
	}
}
