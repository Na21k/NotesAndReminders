using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class MyNotesViewModel : NotesBaseViewModel
	{
		private IDBService _dBService;
		private IAuthorizationService _authorizationService;
		private TrashService _trashService;
		private bool _isRefreshing;

		public ICommand NewNoteCommand { get; private set; }
		public ICommand RefreshCommand { get; private set; }
		public ICommand DisplayNoteActionsCommand { get; private set; }
		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public MyNotesViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();
			_authorizationService = DependencyService.Get<IAuthorizationService>();
			_trashService = new TrashService();

			NewNoteCommand = new Command(NewNoteAsync);
			RefreshCommand = new Command(RefreshAsync);
			DisplayNoteActionsCommand = new Command<Note>(DisplayNoteActionsAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<ArchivedNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<ProfileViewModel>(this, Constants.LoggedOutEvent, OnLoggedOut);

			var n1 = new Note()
			{
				Title = "test title one",
				Text = "test text one",
				Type = new NoteType()
				{
					Name = "Note type test one",
					Color = new NoteColorModel()
					{
						Light = Color.FromHex("#70ff9b"),
						Dark = Color.DarkGray
					}
				},
				State = NoteState.Regular,
				LastEdited = DateTime.UtcNow
			};

			var n2 = new Note()
			{
				Title = "test title 2",
				Text = "test text 2",
				Type = new NoteType()
				{
					Name = "note type test one",
					Color = new NoteColorModel()
					{
						Light = Color.FromHex("#ff7070"),
						Dark = Color.DarkGray
					}
				},
				State = NoteState.Regular,
				LastEdited = DateTime.UtcNow
			};

			var n3 = new Note()
			{
				Title = "test title three",
				Text = "test text 3",
				Type = new NoteType()
				{
					Name = "note type test 2",
					Color = new NoteColorModel()
					{
						Light = Color.FromHex("#ff709b"),
						Dark = Color.DarkGray
					}
				},
				State = NoteState.Regular,
				LastEdited = DateTime.UtcNow
			};

			/*Notes.Add(n1);
			Notes.Add(n2);
			Notes.Add(n3);*/
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

				await _dBService.GetAllNotesAsync(notes =>
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

		private async void NewNoteAsync()
		{
			if (_authorizationService.IsLoggedIn)
			{
				await Shell.Current.GoToAsync($"{nameof(NoteDetailsView)}");
			}
			else
			{
				await Shell.Current.GoToAsync($"{nameof(LogInView)}");
			}
		}

		private async Task ArchiveNoteAsync(Note note)
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

		private async Task DeleteNoteAsync(Note note)
		{
			try
			{
				await _trashService.MoveNoteToTrashAsync(note);
				Notes.Remove(note);
				MessagingCenter.Send(this, Constants.NotesUpdatedEvent);
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
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
				AppResources.MoveToArchive,
				AppResources.Delete
			};

			var res = await Shell.Current.DisplayActionSheet(null, null, null, options);

			if (res == AppResources.MoveToArchive)
			{
				await ArchiveNoteAsync(note);
			}
			else if (res == AppResources.Delete)
			{
				await DeleteNoteAsync(note);
			}
		}

		private async void OnNotesUpdatedAsync(NoteDetailsViewModel vm)
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
		}
	}
}
