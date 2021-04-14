using NotesAndReminders.Models;
using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class MyNotesViewModel : NotesBaseViewModel
	{
		private IDBService _dBService;
		private bool _isRefreshing;

		public ICommand NewNoteCommand { get; private set; }
		public ICommand ArchiveNoteCommand { get; private set; }
		public ICommand RefreshCommand { get; private set; }
		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public MyNotesViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();

			NewNoteCommand = new Command(NewNoteAsync);
			ArchiveNoteCommand = new Command<Note>(ArchiveNoteAsync);
			RefreshCommand = new Command(RefreshAsync);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NotesUpdatedEvent, OnNotesUpdated);

			var n1 = new Note()
			{
				Title = "test title one",
				Text = "test text one",
				Type = new NoteType()
				{
					Name = "Note type test one",
					Color = Color.FromHex("#70ff9b"),
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
					Color = Color.FromHex("#ff7070"),
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
					Color = Color.FromHex("#ff709b"),
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

			IsRefreshing = true;

			await _dBService.GetAllNotesAsync(notes =>
			{
				Notes.Clear();
				notes.ForEach(note => Notes.Add(note as Note));

				IsRefreshing = false;
			});
		}

		private async void NewNoteAsync()
		{
			await Shell.Current.GoToAsync($"{nameof(NoteDetailsView)}");
		}

		private async void ArchiveNoteAsync(Note note)
		{

		}

		private async void RefreshAsync()
		{
			await ReloadDataAsync();
		}

		private async void OnNotesUpdated(NoteDetailsViewModel vm)
		{
			await ReloadDataAsync();
		}
	}
}
