using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NotesAndReminders.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		protected bool SetProperty<T>(ref T backingStore, T value,
			[CallerMemberName] string propertyName = "",
			Action onChanged = null)
		{
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		protected virtual async Task ReloadDataAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{

		}

		public virtual void OnAppearing()
		{

		}

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			var changed = PropertyChanged;
			changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion
	}
}
