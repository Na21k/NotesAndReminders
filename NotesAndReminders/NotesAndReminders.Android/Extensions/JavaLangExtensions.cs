using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;
using NotesAndReminders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotesAndReminders.Droid.Extensions
{
	public static class JavaLangExtensions
	{
		public static IDictionary<string, object> ToDictionary(this IDictionary<string,Java.Lang.Object> map)
		{
			var dict = new Dictionary<string, object>();

			foreach(var key in map.Keys)
			{
			
				var val = map[key];

				if(val is Java.Lang.String str)
				{
					dict.Add(key, str.ToString());
				}
				else if(val is Java.Lang.Double dbl)
				{
					dict.Add(key, dbl.DoubleValue());
				}
				else if(val is Java.Lang.Integer intVal)
				{
					dict.Add(key,intVal.IntValue());
				}
				else if(val is Java.Util.Date dt)
				{
					var format = new SimpleDateFormat("dd/MM/yyyy HH:mm:ss");
					dict.Add(key, format.Format(dt));
				}
				else if(val is Java.Lang.Boolean boolVal)
				{
					dict.Add(key, boolVal.BooleanValue());
				}
				else if(val is System.Collections.ICollection coll)
				{
					var arrList = new ArrayList(coll);
					var list = new List<string>();
					for (int i = 0; i < arrList.Size(); i++)
					{
						list.Add(arrList.Get(i).ToString());
					}
					dict.Add(key, list);
				}
				else
				{
					dict.Add(key, val.ToString());
				}
			}

			return dict;
		}
	}
}