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

		public static readonly List<NoteColorModel> NotesColorsOptions = new List<NoteColorModel>()
		{
			new NoteColorModel(){ Light = Color.Red, Dark = Color.Blue }
		};
	}
}
