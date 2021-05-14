using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace NotesAndReminders.ViewModels
{
	public class SearchViewModel : NotesBaseViewModel
	{
		private IDBService _dBService;
		private TrashService _trashService;
		private string _searchText;
		private ObservableCollection<Note> _foundNotes;

		public string SearchText
		{
			get => _searchText;
			set
			{
				SetProperty(ref _searchText, value);
				UpdateSearchResults();
			}
		}
		public ObservableCollection<Note> FoundNotes
		{
			get => _foundNotes;
			set => SetProperty(ref _foundNotes, value);
		}

		public SearchViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();
			_trashService = new TrashService();
			_searchText = string.Empty;
			FoundNotes = new ObservableCollection<Note>();

			MessagingCenter.Subscribe<NotesBaseViewModel>(this, Constants.SearchOpenedEvent, Init);
		}

		private void Init(NotesBaseViewModel vm)
		{
			Notes = vm.Notes;
			UpdateSearchResults();
		}

		private void UpdateSearchResults()
		{
			FoundNotes.Clear();
			Notes.ForEach(note =>
			{
				if (note.Title.Contains(SearchText) || note.Text.Contains(SearchText))
				{
					FoundNotes.Add(note);
				}
			});
		}

		protected override async Task ArchiveNoteAsync(Note note)
		{
			if (await _dBService.ArchiveNoteAsync(note))
			{
				Notes.Remove(note);
				FoundNotes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			else
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
			}
		}

		protected override async Task UnarchiveNoteAsync(Note note)
		{
			if (await _dBService.UnarchiveNoteAsync(note))
			{
				Notes.Remove(note);
				FoundNotes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			else
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
			}
		}

		protected override async Task DeleteNoteAsync(Note note)
		{
			try
			{
				await _trashService.MoveNoteToTrashAsync(note);
				Notes.Remove(note);
				FoundNotes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
			}
		}
	}
}
