using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NoteDetailsViewModel : BaseViewModel
	{
		public enum NoteDetailsViewType { NewNote, EditNote }

		private NoteDetailsViewType _viewType;
		private IDBService _dBService;
		private TrashService _trashService;
		private Note _note;
		private ObservableCollection<ChecklistItem> _noteChecklistMirror;
		private string _newChecklistItemText;

		public Note Note
		{
			get => _note;
			set => SetProperty(ref _note, value);
		}
		//mirroring as ObservableCollection to fix layout bug
		public ObservableCollection<ChecklistItem> NoteChecklistMirror
		{
			get => _noteChecklistMirror;
			set => SetProperty(ref _noteChecklistMirror, value);
		}
		public string NewChecklistItemText
		{
			get => _newChecklistItemText;
			set => SetProperty(ref _newChecklistItemText, value);
		}

		public ICommand SaveNoteCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }
		public ICommand AddChecklistItemCommand { get; private set; }
		public ICommand DeleteChecklistItemCommand { get; private set; }

		public NoteDetailsViewModel() : base()
		{
			_viewType = NoteDetailsViewType.NewNote;
			_dBService = DependencyService.Get<IDBService>();
			_trashService = new TrashService();

			Note = new Note();
			NoteChecklistMirror = new ObservableCollection<ChecklistItem>();

			SaveNoteCommand = new Command(SaveNoteAsync);
			DeleteNoteCommand = new Command(DeleteNoteAsync);
			AddChecklistItemCommand = new Command(AddChecklistItem);
			DeleteChecklistItemCommand = new Command<ChecklistItem>(DeleteChecklistItem);

			MessagingCenter.Subscribe<NotesBaseViewModel>(this, Constants.NoteDetailsOpenedEvent, InitForExistingNote);
		}

		private void InitForExistingNote(NotesBaseViewModel vm)
		{
			_viewType = NoteDetailsViewType.EditNote;
			Note = vm.SelectedNote;
			Note.Checklist?.ForEach(item => NoteChecklistMirror.Add(item));
		}

		private async void SaveNoteAsync()
		{
			if (Note.Checklist == null)
			{
				Note.Checklist = new List<ChecklistItem>();
			}

			Note.Checklist.Clear();
			Note.Checklist.AddRange(NoteChecklistMirror);
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
				try
				{
					await _trashService.MoveNoteToTrashAsync(Note);
					MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
				}
				catch (Exception ex)
				{
					await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
				}
			}

			await Shell.Current.Navigation.PopToRootAsync();
		}

		private void AddChecklistItem()
		{
			var newItem = new ChecklistItem()
			{
				Text = NewChecklistItemText
			};

			NoteChecklistMirror.Add(newItem);
			NewChecklistItemText = string.Empty;
		}

		private void DeleteChecklistItem(ChecklistItem item)
		{
			NoteChecklistMirror.Remove(item);
		}
	}
}
