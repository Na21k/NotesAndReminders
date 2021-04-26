using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class ArchivedNotesViewModel : NotesBaseViewModel
	{
		private IDBService _dBService;
		private IAuthorizationService _authorizationService;
		private bool _isRefreshing;

		public ICommand RefreshCommand { get; private set; }
		public ICommand DisplayNoteActionsCommand { get; private set; }

		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public ArchivedNotesViewModel()
		{
			_dBService = DependencyService.Get<IDBService>();
			_authorizationService = DependencyService.Get<IAuthorizationService>();

			RefreshCommand = new Command(RefreshAsync);
			DisplayNoteActionsCommand = new Command<Note>(DisplayNoteActionsAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<MyNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
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

			if (_authorizationService.IsLoggedIn)
			{
				IsRefreshing = true;

				await _dBService.GetAllArchivedNotesAsync(notes =>
				{
					Notes.Clear();
					notes.ForEach(note =>
					{
						var nt = note as Note;

						if (nt.Type == null)
						{
							nt.Type = new NoteType()
							{
								Name = AppResources.Uncategorized,
								Color = Constants.NotesColorsOptions.FirstOrDefault()
							};
						}

						Notes.Add(nt);
					});

					IsRefreshing = false;
				});
			}
			else
			{
				IsRefreshing = false;
			}
		}

		private async void RefreshAsync()
		{
			await ReloadDataAsync();
		}

		private async void DisplayNoteActionsAsync(Note note)
		{
			var options = new string[]
			{
				AppResources.Unarchive,
				AppResources.Delete
			};

			var res = await Shell.Current.DisplayActionSheet(null, null, null, options);

			if (res == AppResources.Unarchive)
			{
				await UnarchiveNoteAsync(note);
			}
			else if (res == AppResources.Delete)
			{
				await DeleteNoteAsync(note);
			}
		}

		private async Task UnarchiveNoteAsync(Note note)
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

		private async Task DeleteNoteAsync(Note note)
		{
			if (await _dBService.DeleteNoteAsync(note))
			{
				Notes.Remove(note);
			}
			else
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
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

		private void OnLoggedOut(ProfileViewModel vm)
		{
			Notes.Clear();
		}
	}
}
