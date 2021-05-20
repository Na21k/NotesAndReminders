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

		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public ArchivedNotesViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();
			_authorizationService = DependencyService.Get<IAuthorizationService>();

			RefreshCommand = new Command(RefreshAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<MyNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<TrashViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<SearchViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<ProfileViewModel>(this, Constants.LoggedOutEvent, OnLoggedOut);
			MessagingCenter.Subscribe<NotesBaseViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
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

					notes.Sort((el1, el2) =>
					{
						var n1 = el1 as Note;
						var n2 = el2 as Note;

						return n2.LastEdited.CompareTo(n1.LastEdited);
					});

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

		private async void OnNotesUpdatedAsync(NoteDetailsViewModel vm)
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdatedAsync(MyNotesViewModel vm)
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdatedAsync(TrashViewModel vm)
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdatedAsync(SearchViewModel vm)
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdatedAsync(NotesBaseViewModel vm)
		{
			await ReloadDataAsync();
		}

		private void OnLoggedOut(ProfileViewModel vm)
		{
			Notes.Clear();
		}
	}
}
