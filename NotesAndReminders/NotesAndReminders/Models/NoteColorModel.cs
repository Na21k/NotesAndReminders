using Newtonsoft.Json;
using NotesAndReminders.Converters;
using System.ComponentModel;
using Xamarin.Forms;

namespace NotesAndReminders.Models
{
	public class NoteColorModel : INotifyPropertyChanged
	{
		public Color CurrentThemeColor
		{
			get
			{
				if (Application.Current.RequestedTheme == OSAppTheme.Dark)
				{
					return Dark;
				}
				else
				{
					return Light;
				}
			}
		}
		[JsonConverter(typeof(ColorJsonConverter))]
		public Color Light { get; set; }
		[JsonConverter(typeof(ColorJsonConverter))]
		public Color Dark { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public NoteColorModel()
		{
			Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
		}

		private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentThemeColor)));
		}
	}
}
