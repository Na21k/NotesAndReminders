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

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdated);
			MessagingCenter.Subscribe<MyNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdated);
			MessagingCenter.Subscribe<ArchivedNotesViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdated);
			MessagingCenter.Subscribe<SearchViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdated);
			MessagingCenter.Subscribe<ProfileViewModel>(this, Constants.LoggedOutEvent, OnLoggedOut);
		}

		public override void OnAppearing()
		{
			base.OnAppearing();

			if (Notes.Count == 0)
			{
				IsRefreshing = true;
			}
		}

		protected override async Task ReloadDataAsync()
		{
			await base.ReloadDataAsync();

			var notes = await _trashService.GetTrashedNotesAsync();

			notes.Sort((n1, n2) =>
			{
				return n2.LastEdited.CompareTo(n1.LastEdited);
			});

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

		private void OnNotesUpdated(NoteDetailsViewModel vm)
		{
			IsRefreshing = true;
		}

		private void OnNotesUpdated(MyNotesViewModel vm)
		{
			IsRefreshing = true;
		}

		private void OnNotesUpdated(ArchivedNotesViewModel vm)
		{
			IsRefreshing = true;
		}

		private void OnNotesUpdated(SearchViewModel vm)
		{
			IsRefreshing = true;
		}

		private void OnLoggedOut(ProfileViewModel vm)
		{
			Notes.Clear();
		}
	}
}
