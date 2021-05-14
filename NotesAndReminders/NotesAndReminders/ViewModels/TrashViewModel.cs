using NotesAndReminders.Resources;
using NotesAndReminders.Services;
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
		public ICommand EmptyTrashCommand { get; private set; }

		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public TrashViewModel()
		{
			_trashService = new TrashService();

			RefreshCommand = new Command(RefreshAsync);
			EmptyTrashCommand = new Command(EmptyTrashAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<MyNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<ArchivedNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
			MessagingCenter.Subscribe<SearchViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdatedAsync);
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

		private async void EmptyTrashAsync()
		{
			if (await Shell.Current.DisplayAlert(null, AppResources.AreYouSureToEmptyTrash, AppResources.Yes, AppResources.No))
			{
				_trashService.EmptyTrash();
				Notes.Clear();
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

		private async void OnNotesUpdatedAsync(SearchViewModel vm)
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
