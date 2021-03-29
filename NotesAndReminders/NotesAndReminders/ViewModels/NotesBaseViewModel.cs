﻿using NotesAndReminders.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NotesBaseViewModel : BaseViewModel
	{
		private ObservableCollection<Note> _notes;

		public ObservableCollection<Note> Notes
		{
			get => _notes;
			set => SetProperty(ref _notes, value);
		}

		public ICommand OpenNoteCommand { get; private set; }
		public ICommand DeleteNoteCommand { get; private set; }

		public NotesBaseViewModel()
		{
			Notes = new ObservableCollection<Note>();

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
