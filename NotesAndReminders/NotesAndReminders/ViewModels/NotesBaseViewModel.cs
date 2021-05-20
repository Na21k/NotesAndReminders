using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NotesBaseViewModel : BaseViewModel
	{
		private IDBService _dBService;
		private TrashService _trashService;
		private ObservableCollection<Note> _notes;

		public ObservableCollection<Note> Notes
		{
			get => _notes;
			set => SetProperty(ref _notes, value);
		}

		public Note SelectedNote { get; private set; }

		public ICommand SearchCommand { get; private set; }
		public ICommand OpenNoteCommand { get; private set; }
		public ICommand DisplayNoteActionsCommand { get; private set; }

		public NotesBaseViewModel()
		{
			_dBService = DependencyService.Get<IDBService>();
			_trashService = new TrashService();
			Notes = new ObservableCollection<Note>();

			SearchCommand = new Command(SearchAsync);
			OpenNoteCommand = new Command<Note>(OpenNoteAsync);
			DisplayNoteActionsCommand = new Command<Note>(DisplayNoteActionsAsync);
		}

		private async void SearchAsync()
		{
			await Shell.Current.GoToAsync(nameof(SearchView));
			MessagingCenter.Send(this, Constants.SearchOpenedEvent);
		}

		private async void OpenNoteAsync(Note note)
		{
			SelectedNote = note;

			await Shell.Current.GoToAsync($"{nameof(NoteDetailsView)}");
			MessagingCenter.Send(this, Constants.NoteDetailsOpenedEvent);
		}

		private async void DisplayNoteActionsAsync(Note note)
		{
			var options = new List<string>();

			switch (note.State)
			{
				case NoteState.Regular:
					options.Add(AppResources.MoveToArchive);
					options.Add(AppResources.SetCategory);
					break;
				case NoteState.Archived:
					options.Add(AppResources.Unarchive);
					options.Add(AppResources.SetCategory);
					break;
				default:
					options.Add(AppResources.Restore);
					break;
			}

			options.Insert(1, AppResources.Delete);

			var res = await Shell.Current.DisplayActionSheet(null, null, null, options.ToArray());

			if (res == AppResources.MoveToArchive)
			{
				await ArchiveNoteAsync(note);
			}
			else if (res == AppResources.Unarchive)
			{
				await UnarchiveNoteAsync(note);
			}
			else if (res == AppResources.Restore)
			{
				await RestoreNoteAsync(note);
			}
			else if (res == AppResources.SetCategory)
			{
				await SetCategory(note);
			}
			else if (res == AppResources.Delete)
			{
				await DeleteNoteAsync(note);
			}
		}

		protected virtual async Task ArchiveNoteAsync(Note note)
		{
			if (await _dBService.ArchiveNoteAsync(note))
			{
				Notes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			else
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
			}
		}

		protected virtual async Task UnarchiveNoteAsync(Note note)
		{
			if (await _dBService.UnarchiveNoteAsync(note))
			{
				Notes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			else
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
			}
		}

		protected virtual async Task RestoreNoteAsync(Note note)
		{
			try
			{
				await _trashService.RestoreNoteFromTrashAsync(note);
				Notes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
			}
		}

		protected virtual async Task DeleteNoteAsync(Note note)
		{
			try
			{
				if (note.State == NoteState.Trashed)
				{
					await _trashService.DeleteNoteFromTrash(note);
				}
				else
				{
					await _trashService.MoveNoteToTrashAsync(note);
				}

				Notes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
			}
		}

		private async Task SetCategory(Note note)
		{
			await _dBService.GetAllNoteTypesAsync(async noteTypes =>
			{
				var options = noteTypes.Select(nt => (nt as NoteType).Name).ToList();
				options.Insert(0, AppResources.Uncategorized);

				var res = await Shell.Current.DisplayActionSheet(null, null, null, options.ToArray());

				if (res == null)
				{
					return;
				}
				else if (res == AppResources.Uncategorized)
				{
					note.Type = null;
					await _dBService.UpdateNoteAsync(note);
				}
				else
				{
					var typeDbItem = noteTypes.Where(nt => (nt as NoteType).Name == res).FirstOrDefault();
					var type = typeDbItem as NoteType;
					note.Type = type;
					await _dBService.UpdateNoteAsync(note);
				}

				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			});
		}
	}
}
