using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
		}

		private void RemirrorImagesFromCurrentNote()
		{
			NoteImagesMirror.Clear();

			Note.Images?.ForEach(item =>
			{
				var image = new ImageModel(item);
				NoteImagesMirror.Add(image);
			});
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
					var stream = await file.OpenReadAsync();
					var image = new ImageModel()
					{
						ImageBytes = new byte[stream.Length]
					};
					await stream.ReadAsync(image.ImageBytes, 0, (int)stream.Length);

					NoteImagesMirror.Add(image);
					AddImageToNote(image);
				}
			}
			catch (Exception ex)
			{
				await Shell.Current.DisplayAlert(AppResources.Oops, $"{AppResources.UnexpectedErrorHasOccurred}: {ex.Message}", AppResources.Ok);
			}
		}

		private void DeleteImage(ImageModel image)
		{
			var index = Note.Images.FindIndex(el => el.Equals(image.ImageBytes));

			if (index > -1)
			{
				Note.Images.RemoveAt(index);
				NoteImagesMirror.Remove(image);
			}
		}

		private void AddImageToNote(ImageModel image)
		{
			if (Note.Images == null)
			{
				Note.Images = new List<byte[]>();
			}

			Note.Images.Add(image.ImageBytes);
		}

		public struct ImageModel
		{
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

			public ImageModel(byte[] imageBytes)
			{
				ImageBytes = imageBytes;
			}
		}
	}
}
