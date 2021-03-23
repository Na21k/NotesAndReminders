using System;

namespace NotesAndReminders.Exceptions
{
	public class InvalidCredentialsException : Exception
	{
		public new string Message { get; private set; }
		public new string StackTrace { get; private set; }

		public InvalidCredentialsException(string message, string stacktrace)
		{
			Message = message;
			StackTrace = stacktrace;
		}
	}
}
