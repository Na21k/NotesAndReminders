using NotesAndReminders.Models;
using NotesAndReminders.Resources;
using NotesAndReminders.Services;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NewOrEditCategoryViewModel : BaseViewModel
	{
		private IDBService _dBService;
		private string _categoryName;
		private NoteColorModel _selectedColorsOption;
		private bool _isEditing;
		private bool _isSaving;

		private NoteType NoteType { get; set; }
		public string CategoryName
		{
			get => _categoryName;
			set
			{
				SetProperty(ref _categoryName, value);
				NoteType.Name = value;
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
				NoteType.Color = value;
				OnPropertyChanged(nameof(CanSave));
			}
		}
		public bool IsEditing
		{
			get => _isEditing;
			set => SetProperty(ref _isEditing, value);
		}
		public bool IsSaving
		{
			get => _isSaving;
			set
			{
				SetProperty(ref _isSaving, value);
			}
		}
		public bool CanSave => IsAnyOptionSelected && IsNameProvided;
		private bool IsAnyOptionSelected => SelectedColorsOption != null && NoteType.Color != null;
		private bool IsNameProvided => !string.IsNullOrWhiteSpace(CategoryName) && !string.IsNullOrWhiteSpace(NoteType?.Name);

		public ICommand SaveCategoryCommand { get; private set; }

		public NewOrEditCategoryViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();
			NoteType = new NoteType();

			SaveCategoryCommand = new Command(SaveCategoryAsync);

			MessagingCenter.Subscribe<NotesCategoriesViewModel>(this, Constants.EditCategoryEvent, InitForExisting);
		}

		private void InitForExisting(NotesCategoriesViewModel vm)
		{
			NoteType = vm.NoteTypeForEditing;
			CategoryName = NoteType.Name;
			IsEditing = true;
		}

		private async void SaveCategoryAsync()
		{
			IsSaving = true;

			if (IsEditing)
			{
				if (await _dBService.UpdateNoteTypeAsync(NoteType))
				{
					MessagingCenter.Send(this, Constants.NotesCategoriesUpdatedEvent);
					await Shell.Current.Navigation.PopToRootAsync();
				}
				else
				{
					await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
				}
			}
			else
			{
				if (await _dBService.AddNoteTypeAsync(NoteType))
				{
					MessagingCenter.Send(this, Constants.NotesCategoriesUpdatedEvent);
					await Shell.Current.Navigation.PopToRootAsync();
				}
				else
				{
					await Shell.Current.DisplayAlert(AppResources.Oops, AppResources.UnexpectedErrorHasOccurred, AppResources.Ok);
				}
			}

			IsSaving = false;
		}
	}
}
