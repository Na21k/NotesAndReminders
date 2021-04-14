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
					var format = new SimpleDateFormat();
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

		public static Dictionary<string, Java.Lang.Object> Convert(this Dictionary<string,object> item)
		{
			var dict = new Dictionary<string, Java.Lang.Object>();

			var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(item);
			var propDict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);

			foreach(var key in propDict.Keys)
			{
				var val = propDict[key];
				Java.Lang.Object javaVal = null;
				if(val is string str)
				{
					javaVal = new Java.Lang.String(str);
				}
				else if(val is double dbl)
				{
					javaVal = new Java.Lang.Double(dbl);
				}
				else if(val is int intVal)
				{
					javaVal = new Java.Lang.Integer(intVal);
				}
				else if(val is DateTime dt)
				{
					javaVal = dt.ToString();
				}
				else if(val is bool boolVal)
				{
					javaVal = new Java.Lang.Boolean(boolVal);
				}
				
				if(javaVal != null)
				{
					dict.Add(key, javaVal);
				}
			}

			return dict;
		}
	}
}