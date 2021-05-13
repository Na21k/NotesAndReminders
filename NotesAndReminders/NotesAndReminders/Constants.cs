using NotesAndReminders.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NotesAndReminders
{
	public class Constants
	{
		public const string UnexpectedErrorEvent = "UnexpectedErrorEvent";
		public const string LoggedInEvent = "LoggedInEvent";
		public const string LoggedOutEvent = "LoggedOutEvent";
		public const string InvalidLoginOrPasswordEvent = "InvalidLoginOrPasswordEvent";
		public const string EmptyLoginOrPasswordEvent = "EmptyLoginOrPasswordEvent";
		public const string PasswordFieldDoesNotMatchConfirmPasswordFieldEvent = "PasswordFieldDoesNotMatchConfirmPasswordFieldEvent";
		public const string NoteDetailsOpenedEvent = nameof(NoteDetailsOpenedEvent);
		public const string NotesUpdatedEvent = nameof(NotesUpdatedEvent);
		public const string NotesCategoriesUpdatedEvent = nameof(NotesCategoriesUpdatedEvent);
		public const string NoteImagesOpenedEvent = nameof(NoteImagesOpenedEvent);
		public const string HideEmptyTrashButton = nameof(HideEmptyTrashButton);

		public static readonly List<NoteColorModel> NotesColorsOptions = new List<NoteColorModel>()
		{
			new NoteColorModel() { Light = Color.FromHex("#f2f2f2"), Dark = Color.FromHex("#6e6e6e") },	//default gray color
			new NoteColorModel() { Light = Color.FromHex("#ff4040"), Dark = Color.FromHex("#d13030") },	//red
			new NoteColorModel() { Light = Color.FromHex("#ffd54a"), Dark = Color.FromHex("#dbb842") },	//yellow
			new NoteColorModel() { Light = Color.FromHex("#98ff4a"), Dark = Color.FromHex("#7dd13d") },	//green
			new NoteColorModel() { Light = Color.FromHex("#4dffd8"), Dark = Color.FromHex("#38c9aa") },	//cyan1
			new NoteColorModel() { Light = Color.FromHex("#45ffef"), Dark = Color.FromHex("#33bdb1") },	//cyan 2
			new NoteColorModel() { Light = Color.FromHex("#45c7ff"), Dark = Color.FromHex("#38a3d1") },	//blue1
			new NoteColorModel() { Light = Color.FromHex("#4a62ff"), Dark = Color.FromHex("#4a62ff") },	//blue2
			new NoteColorModel() { Light = Color.FromHex("#e83bff"), Dark = Color.FromHex("#8d219c") },	//purple
			new NoteColorModel() { Light = Color.FromHex("#ff2ba0"), Dark = Color.FromHex("#bf247a") },	//pink
			new NoteColorModel() { Light = Color.FromHex("#ff365e"), Dark = Color.FromHex("#c22544") }	//cherryRed
		};
	}
}
