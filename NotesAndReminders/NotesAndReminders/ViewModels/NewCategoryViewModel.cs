using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NewCategoryViewModel : BaseViewModel
	{
		private IDBService _dBService;
		private string _categoryName;
		private NoteColorModel _selectedColorsOption;
		private bool _isAdding;

		public string CategoryName
		{
			get => _categoryName;
			set
			{
				SetProperty(ref _categoryName, value);
				OnPropertyChanged(nameof(CanSave));
			}
		}
		public List<NoteColorModel> ColorsOptions => Constants.NotesColorsOptions;
		public NoteColorModel SelectedColorsOption
		{
			get => _selectedColorsOption;
			set
			{
				SetProperty(ref _selectedColorsOption, value);
				OnPropertyChanged(nameof(CanSave));
			}
		}
		public bool CanSave => IsAnyOptionSelected && IsNameProvided;
		public bool IsAdding
		{
			get => _isAdding;
			set
			{
				SetProperty(ref _isAdding, value);
			}
		}
		private bool IsAnyOptionSelected => SelectedColorsOption != null;
		private bool IsNameProvided => !string.IsNullOrWhiteSpace(CategoryName);

		public ICommand AddCategoryCommand { get; private set; }

		public NewCategoryViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();

			AddCategoryCommand = new Command(AddCategoryAsync);
		}

		private async void AddCategoryAsync()
		{
			IsAdding = true;

			var noteType = new NoteType()
			{
				Name = CategoryName,
				Color = SelectedColorsOption
			};

			if (await _dBService.AddNoteTypeAsync(noteType))
			{
				MessagingCenter.Send(this, Constants.NotesCategoriesUpdatedEvent);
				await Shell.Current.Navigation.PopToRootAsync();
			}
			else
			{
				MessagingCenter.Send(this, Constants.UnexpectedErrorEvent);
			}

			IsAdding = false;
		}
	}
}
