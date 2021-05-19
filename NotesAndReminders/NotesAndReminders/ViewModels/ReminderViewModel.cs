using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class ReminderViewModel : BaseViewModel
	{
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

		public ReminderViewModel() : base()
		{
			Time = DateTime.Now.AddHours(1).TimeOfDay;
			Date = DateTime.Now.AddDays(1).Date;

			SaveCommand = new Command(SaveAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.ManageNoteReminderOpened, Init);
		}

		private void Init(NoteDetailsViewModel vm)
		{
			_note = vm.Note;
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


		}
	}
}
