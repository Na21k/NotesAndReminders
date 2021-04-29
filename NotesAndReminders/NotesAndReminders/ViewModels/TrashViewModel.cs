using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class TrashViewModel : NotesBaseViewModel
	{
		private TrashService _trashService;
		private bool _isRefreshing;

		public ICommand RefreshCommand { get; private set; }
		public ICommand DisplayNoteActionsCommand { get; private set; }

		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public TrashViewModel()
		{
			_trashService = new TrashService();

			RefreshCommand = new Command(RefreshAsync);
			DisplayNoteActionsCommand = new Command<Note>(DisplayNoteActionsAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<MyNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<ArchivedNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<ProfileViewModel>(this, Constants.LoggedOutEvent, OnLoggedOut);
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();

			if (Notes.Count == 0)
			{
				await ReloadDataAsync();
			}
		}

		protected override async Task ReloadDataAsync()
		{
			await base.ReloadDataAsync();

			var notes = await _trashService.GetTrashedNotesAsync();
			Notes.Clear();
			notes.ForEach((note) => Notes.Add(note));

			IsRefreshing = false;
		}

		private async void RefreshAsync()
		{
			await ReloadDataAsync();
		}

		private async void DisplayNoteActionsAsync(Note note)
		{
			var options = new string[]
			{
				AppResources.Restore,
				AppResources.Delete
			};

			var res = await Shell.Current.DisplayActionSheet(null, null, null, options);

			if (res == AppResources.Restore)
			{
				await RestoreNoteAsync(note);
			}
			else if (res == AppResources.Delete)
			{
				await DeleteNoteAsync(note);
			}
		}

		private async Task RestoreNoteAsync(Note note)
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

		private async Task DeleteNoteAsync(Note note)
		{
			try
			{
				await _trashService.DeleteNoteFromTrash(note);
				Notes.Remove(note);
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
			}
		}

		private async void OnNotesUpdatedAsync(NoteDetailsViewModel vm)
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdatedAsync(MyNotesViewModel vm)
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdatedAsync(ArchivedNotesViewModel vm)
		{
			await ReloadDataAsync();
		}

		private void OnLoggedOut(ProfileViewModel vm)
		{
			Notes.Clear();
			_trashService.EmptyTrash();
		}
	}
}
