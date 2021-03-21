using Newtonsoft.Json;
using NotesAndReminders.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;

namespace NotesAndReminders
{
	public static class Settings
	{
		private static Dictionary<string, object> _cache = new Dictionary<string, object>();

		public static User User { get => Get<User>(); set => Set(value: value); }

		private static T Get<T>([CallerMemberName] string name = null, T defaultValue = default)
		{
			if (_cache.TryGetValue(name, out object res))
			{
				if (res is T)
				{
					return (T)res;
				}
				else
				{
					throw new InvalidCastException($"{typeof(Settings)}: failed to convert value of {name} to {typeof(T)}");
				}
			}
			else
			{
				var jsonContent = Preferences.Get(name, string.Empty);

				if (string.IsNullOrWhiteSpace(jsonContent))
				{
					return defaultValue;
				}
				else
				{
					var result = JsonConvert.DeserializeObject<T>(jsonContent);
					_cache[name] = result;

					return result;
				}
			}
		}

		private static void Set<T>([CallerMemberName] string name = null, T value = default)
		{
			var jsonContent = JsonConvert.SerializeObject(value);
			Preferences.Set(name, jsonContent);
			_cache[name] = value;
		}
	}
}
