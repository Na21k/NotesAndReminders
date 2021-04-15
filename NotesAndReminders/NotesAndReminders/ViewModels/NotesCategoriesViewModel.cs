using NotesAndReminders.Models;
using NotesAndReminders.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace NotesAndReminders.ViewModels
{
	public class NotesCategoriesViewModel : BaseViewModel
	{
		private IDBService _dBService;
		private bool _isRefreshing;

		private ObservableCollection<NoteType> _categories;

		public ObservableCollection<NoteType> Categories
		{
			get => _categories;
			set => SetProperty(ref _categories, value);
		}
		public bool IsRefreshing
		{
			get => _isRefreshing;
			set => SetProperty(ref _isRefreshing, value);
		}

		public ICommand ItemTappedCommand { get; private set; }
		public ICommand DeleteCategoryCommand { get; private set; }
		public ICommand RefreshCommand { get; private set; }

		public NotesCategoriesViewModel() : base()
		{
			_dBService = DependencyService.Get<IDBService>();

			Categories = new ObservableCollection<NoteType>();

			ItemTappedCommand = new Command<NoteType>(ItemTappedAsync);
			DeleteCategoryCommand = new Command<NoteType>(DeleteCategoryAsync);
			RefreshCommand = new Command(RefreshAsync);

			/*Categories.Add(new NoteType() { Name = "test", Color = Color.Red });
			Categories.Add(new NoteType() { Name = "tests", Color = Color.Cyan });
			Categories.Add(new NoteType() { Name = "test7777777777", Color = Color.Purple });
			Categories.Add(new NoteType() { Name = "testrrr vrvs f", Color = Color.Black });
			Categories.Add(new NoteType() { Name = "testrrr vrvs", Color = Color.Blue });*/
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

			IsRefreshing = true;

			await _dBService.GetAllNoteTypesAsync(noteTypes =>
			{
				Categories.Clear();
				noteTypes.ForEach(noteType => Categories.Add(noteType as NoteType));

				IsRefreshing = false;
			});
		}

		private async void ItemTappedAsync(NoteType item)
		{
			//for testing
			await _dBService.AddNoteTypeAsync(new NoteType() { Name = "test", Color = Color.Red });
			await _dBService.AddNoteTypeAsync(new NoteType() { Name = "tests", Color = Color.Cyan });
			await _dBService.AddNoteTypeAsync(new NoteType() { Name = "test7777777777", Color = Color.Purple });
			await _dBService.AddNoteTypeAsync(new NoteType() { Name = "testrrr vrvs f", Color = Color.Black });
			await _dBService.AddNoteTypeAsync(new NoteType() { Name = "testrrr vrvs", Color = Color.Blue });
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
	}
}
