using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace NotesAndReminders.Converters
{
	public class ColorJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == Color.Default.GetType();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var color = Color.FromHex(existingValue as string);

			return color;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value is Color color)
			{
				serializer.Serialize(writer, color.ToHex());
			}
		}
	}
}
