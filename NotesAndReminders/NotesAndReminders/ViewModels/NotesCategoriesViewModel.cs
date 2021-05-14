using NotesAndReminders.Models;
using NotesAndReminders.Services;
using NotesAndReminders.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NotesCategoriesViewModel : BaseViewModel
	{
		private IDBService _dBService;
		private IAuthorizationService _authorizationService;
		private bool _isRefreshing;

		private ObservableCollection<NoteType> _categories;

		public ObservableCollection<NoteType> Categories
		{
			get => _categories;
			set => SetProperty(ref _categories, value);
		}
		public NoteType NoteTypeForEditing { get; private set; }
		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public ICommand ItemTappedCommand { get; private set; }
		public ICommand NewCategoryCommand { get; private set; }
		public ICommand DeleteCategoryCommand { get; private set; }
		public ICommand RefreshCommand { get; private set; }

		public NotesCategoriesViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();
			_authorizationService = DependencyService.Get<IAuthorizationService>();

			Categories = new ObservableCollection<NoteType>();

			ItemTappedCommand = new Command<NoteType>(ItemTappedAsync);
			NewCategoryCommand = new Command(NewCategoryAsync);
			DeleteCategoryCommand = new Command<NoteType>(DeleteCategoryAsync);
			RefreshCommand = new Command(RefreshAsync);

			MessagingCenter.Subscribe<NewOrEditCategoryViewModel>(this, Constants.NotesCategoriesUpdatedEvent, OnNotesCategoriesUpdatedAsync);
			MessagingCenter.Subscribe<ProfileViewModel>(this, Constants.LoggedOutEvent, OnLoggedOut);
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();

			if (Categories.Count == 0)
			{
				await ReloadDataAsync();
			}
		}

		protected override async Task ReloadDataAsync()
		{
			await base.ReloadDataAsync();

			if (_authorizationService.IsLoggedIn)
			{
				IsRefreshing = true;

				await _dBService.GetAllNoteTypesAsync(noteTypes =>
				{
					Categories.Clear();
					noteTypes.ForEach(noteType => Categories.Add(noteType as NoteType));

					IsRefreshing = false;
				});
			}
			else
			{
				IsRefreshing = false;
			}
		}

		private async void ItemTappedAsync(NoteType item)
		{
			NoteTypeForEditing = item;
			await Shell.Current.GoToAsync(nameof(NewOrEditCategoryView));
			MessagingCenter.Send(this, Constants.EditCategoryEvent);
		}

		private async void NewCategoryAsync()
		{
			if (_authorizationService.IsLoggedIn)
			{
				await Shell.Current.GoToAsync(nameof(NewOrEditCategoryView));
			}
			else
			{
				await Shell.Current.GoToAsync(nameof(LogInView));
			}
		}

		private async void DeleteCategoryAsync(NoteType item)
		{
			if (await _dBService.DeleteNoteTypeAsync(item))
			{
				Categories.Remove(item);
			}
			else
			{
				MessagingCenter.Send(this, Constants.UnexpectedErrorEvent);
			}
		}

		private async void RefreshAsync()
		{
			await ReloadDataAsync();
		}

		private async void OnNotesCategoriesUpdatedAsync(NewOrEditCategoryViewModel vm)
		{
			await ReloadDataAsync();
		}

		private void OnLoggedOut(ProfileViewModel vm)
		{
			Categories.Clear();
		}
	}
}
