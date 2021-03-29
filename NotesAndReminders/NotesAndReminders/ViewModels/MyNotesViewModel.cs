using NotesAndReminders.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class MyNotesViewModel : NotesBaseViewModel
	{
		public ICommand NewNoteCommand { get; private set; }
		public ICommand ArchiveNoteCommand { get; private set; }

		public MyNotesViewModel() : base()
		{
			NewNoteCommand = new Command(NewNoteAsync);
			ArchiveNoteCommand = new Command<Note>(ArchiveNoteAsync);

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

			Notes.Add(n1);
			Notes.Add(n2);
			Notes.Add(n3);
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();

			await ReloadDataAsync();
		}

		protected override async Task ReloadDataAsync()
		{
			await base.ReloadDataAsync();


		}

		private async void NewNoteAsync()
		{

		}

		private async void ArchiveNoteAsync(Note note)
		{

		}
	}
}
