using NotesAndReminders.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NotesBaseViewModel : BaseViewModel
	{
		private List<Note> _notes;

		public List<Note> Notes
		{
			get => _notes;
			set => SetProperty(ref _notes, value);
		}

		public ICommand OpenNoteCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }

		public NotesBaseViewModel()
		{
			Notes = new List<Note>();

			OpenNoteCommand = new Command<Note>(OpenNoteAsync);
			DeleteNoteCommand = new Command<Note>(DeleteNoteAsync);
		}

		private async void OpenNoteAsync(Note note)
		{

		}

		private async void DeleteNoteAsync(Note note)
		{

		}
	}
}
