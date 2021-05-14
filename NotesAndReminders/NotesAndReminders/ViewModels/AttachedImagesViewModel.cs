using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class AttachedImagesViewModel : BaseViewModel
	{
		private Note _note;
		private ObservableCollection<ImageModel> _noteImagesMirror;

		public Note Note
		{
			get => _note;
			set => SetProperty(ref _note, value);
		}
		public ObservableCollection<ImageModel> NoteImagesMirror
		{
			get => _noteImagesMirror;
			set => SetProperty(ref _noteImagesMirror, value);
		}
		public bool IsEmpty => NoteImagesMirror.Count == 0;

		public ICommand AddImageCommand { get; private set; }
		public ICommand DeleteImageCommand { get; private set; }

		public AttachedImagesViewModel() : base()
		{
			Note = new Note();
			NoteImagesMirror = new ObservableCollection<ImageModel>();

			AddImageCommand = new Command(AddImageAsync);
			DeleteImageCommand = new Command<ImageModel>(DeleteImage);

			MessagingCenter.Subscribe<NoteDetailsViewModel>(this, Constants.NoteImagesOpenedEvent, Init);
		}

		private void Init(NoteDetailsViewModel vm)
		{
			Note = vm.Note;
			RemirrorImagesFromCurrentNote();

			if (Note.State == NoteState.Trashed || Note.State == NoteState.ArchivedTrashed)
			{
				MessagingCenter.Send(this, Constants.HideSaveAndDeleteButtonsEvent);
			}
		}

		private void RemirrorImagesFromCurrentNote()
		{
			NoteImagesMirror.Clear();

			if (Note.Images != null)
			{
				foreach (var item in Note.Images)
				{
					var image = new ImageModel(item.Key, item.Value);
					NoteImagesMirror.Add(image);
				}
			}

			OnPropertyChanged(nameof(IsEmpty));
		}

		private async void AddImageAsync()
		{
			try
			{
				var file = await FilePicker.PickAsync(new PickOptions()
				{
					FileTypes = FilePickerFileType.Images
				});

				if (file != null)
				{
					if (Note.Images != null && Note.Images.ContainsKey(file.FileName))
					{
						await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.ThisFileHasAlreadyBeenAdded, AppResources.Ok);

						return;
					}

					var stream = await file.OpenReadAsync();
					var image = new ImageModel(file.FileName, new byte[stream.Length]);
					await stream.ReadAsync(image.ImageBytes, 0, (int)stream.Length);

					NoteImagesMirror.Add(image);
					AddImageToNote(image);

					OnPropertyChanged(nameof(IsEmpty));
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
			}
		}

		private void DeleteImage(ImageModel image)
		{
			var found = Note.Images.FirstOrDefault(kv => kv.Value.Equals(image.ImageBytes));

			if (!found.Equals(default))
			{
				Note.Images.Remove(found.Key);
				NoteImagesMirror.Remove(image);

				OnPropertyChanged(nameof(IsEmpty));
			}
		}

		private void AddImageToNote(ImageModel image)
		{
			if (Note.Images == null)
			{
				Note.Images = new Dictionary<string, byte[]>();
			}

			Note.Images.Add(image.FileName, image.ImageBytes);
		}

		public struct ImageModel
		{
			public string FileName { get; set; }
			public byte[] ImageBytes { get; set; }
			public ImageSource ImageSource
			{
				get
				{
					var stream = new MemoryStream(ImageBytes);
					var imageSource = ImageSource.FromStream(() => stream);

					return imageSource;
				}
			}

			public ImageModel(string fileName, byte[] imageBytes)
			{
				FileName = fileName;
				ImageBytes = imageBytes;
			}
		}
	}
}
