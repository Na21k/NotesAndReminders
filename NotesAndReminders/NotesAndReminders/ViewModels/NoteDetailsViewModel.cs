using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NoteDetailsViewModel : BaseViewModel
	{
		public enum NoteDetailsViewType { NewNote, EditNote }

		private NoteDetailsViewType _viewType;
		private IDBService _dBService;
		private Note _note;

		public Note Note
		{
			get => _note;
			set => SetProperty(ref _note, value);
		}

		public ICommand SaveNoteCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }

		public NoteDetailsViewModel() : base()
		{
			_viewType = NoteDetailsViewType.NewNote;
			_dBService = DependencyService.Get<IDBService>();

			Note = new Note();

			SaveNoteCommand = new Command(SaveNoteAsync);
			DeleteNoteCommand = new Command(DeleteNoteAsync);

			MessagingCenter.Subscribe<NotesBaseViewModel>(this, Constants.NoteDetailsOpenedEvent, InitForExistingNote);
		}

		private void InitForExistingNote(NotesBaseViewModel vm)
		{
			_viewType = NoteDetailsViewType.EditNote;
			Note = vm.SelectedNote;
		}

		private async void SaveNoteAsync()
		{
			Note.LastEdited = DateTime.Now;

			switch (_viewType)
			{
				case NoteDetailsViewType.NewNote:
					await _dBService.AddNoteAsync(Note);
					break;
				case NoteDetailsViewType.EditNote:
					await _dBService.UpdateNoteAsync(Note);
					break;
			}

			MessagingCenter.Send(this, Constants.NotesUpdatedEvent);

			await Shell.Current.Navigation.PopToRootAsync();
		}

		private async void DeleteNoteAsync()
		{
			if (_viewType == NoteDetailsViewType.EditNote)
			{
				await _dBService.DeleteNoteAsync(Note);
			}

			MessagingCenter.Send(this, Constants.NotesUpdatedEvent);

			await Shell.Current.Navigation.PopToRootAsync();
		}
	}
}
