using Firebase.Firestore;
using NotesAndReminders.Droid.Extensions;
using NotesAndReminders.Droid.Services;
using NotesAndReminders.Models;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace NotesAndReminders.Droid
{
	public class OnCompleteListener<T> : Java.Lang.Object, Android.Gms.Tasks.IOnCompleteListener where T : Identifiable, IDBItem
	{
		private Action<IDBItem> _onCompleteCallback;

		public OnCompleteListener(Action<IDBItem> onCompleteCallback)
		{
			_onCompleteCallback = onCompleteCallback;

		}
		private static T Convert(DocumentSnapshot doc)
		{
			Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings
			{
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
				Formatting = Newtonsoft.Json.Formatting.Indented
			};
			try
			{
				var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(doc.Data.ToDictionary(), settings);
				var item = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr, settings);
				item.Id = doc.Id;
				if (item is NoteType nt)
				{
					var data = (JObject)JsonConvert.DeserializeObject(jsonStr, settings);
					var lcolor = data["noteColorLight"].Value<string>();
					var dcolor = data["noteColorDark"].Value<string>();
					var color = Color.FromHex(lcolor);

					nt.Color = new NoteColorModel();
					nt.Color.Light = color;

					color = Color.FromHex(dcolor);
					nt.Color.Dark = color;

					return (T)System.Convert.ChangeType(nt, typeof(T));
				}
				else
				{
					return item;
				}

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public void OnComplete(Android.Gms.Tasks.Task task)
		{
			if (task.IsSuccessful)
			{
				var docObj = task.Result;
				if (docObj is DocumentSnapshot docSnap && docSnap.Exists())
				{
					var item = Convert(docSnap);
					_onCompleteCallback?.Invoke(item);
				}
				else
				{
					throw new Exception("No such documment exists");
				}
			}
			else
			{
				throw new Exception("Failed to get documment");
			}
		}
	}

	public class OnCompleteListListener<T> : Java.Lang.Object, Android.Gms.Tasks.IOnCompleteListener where T : Identifiable, IDBItem
	{
		private Action<List<IDBItem>> _onCompleteCallback;

		public OnCompleteListListener(Action<List<IDBItem>> onCompleteCallback)
		{
			_onCompleteCallback = onCompleteCallback;
		}
		private static T Convert(DocumentSnapshot doc)
		{
			Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings
			{
				TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
				Formatting = Newtonsoft.Json.Formatting.Indented
			};
			try
			{
				var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(doc.Data.ToDictionary(), settings);
				var item = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr, settings);
				item.Id = doc.Id;
				if (item is NoteType nt)
				{
					var data = (JObject)JsonConvert.DeserializeObject(jsonStr,settings);
					var lcolor = data["noteColorLight"].Value<string>();
					var dcolor = data["noteColorDark"].Value<string>();
					var color = Color.FromHex(lcolor);

					nt.Color = new NoteColorModel();
					nt.Color.Light = color;

					color = Color.FromHex(dcolor);
					nt.Color.Dark = color;

					return (T)System.Convert.ChangeType(nt, typeof(T));
				}
				else
				{
					return item;
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.StackTrace);

				throw;
			}
		}

		public async void OnComplete(Android.Gms.Tasks.Task task)
		{
			if (task.IsSuccessful)
			{
				var models = new List<IDBItem>();

				var collObj = task.Result;
				if (collObj is QuerySnapshot collQuery)
				{

					foreach (var item in collQuery.Documents)
					{
						var convertedItem = Convert(item);
						if (convertedItem is Note note)
						{
							if (note.NoteTypeId != null)
							{
								FirebaseCloudFirestoreService service = new FirebaseCloudFirestoreService();
								var nt = new NoteType();
								await service.GetNoteTypeAsync(note.NoteTypeId, notetype =>
								{
									nt = notetype as NoteType;
								});

								note.Type = nt;
							}

							models.Add(note);
						}
						else
						{
							models.Add(convertedItem);
						}

					}

					_onCompleteCallback?.Invoke(models);
				}
				else
				{
					throw new Exception("No such collection exists");
				}
			}
			else
			{
				throw new Exception("Failed to get collection");
			}
		}
	}

	public class OnUserCompleteListener<T> : Java.Lang.Object, Android.Gms.Tasks.IOnCompleteListener where T : Identifiable, IDBItem
	{
		private Action<IDBItem> _onUserCompleteCallback;
		public OnUserCompleteListener(Action<IDBItem> onUserCompleteCallback)
		{
			_onUserCompleteCallback = onUserCompleteCallback;

		}

		public void OnComplete(Android.Gms.Tasks.Task task)
		{
			if (task.IsSuccessful)
			{
				var docObj = task.Result;
				if (docObj is DocumentSnapshot docSnap && docSnap.Exists())
				{
					var user = new User
					{
						Id = docSnap.Id,
						UserName = docSnap.GetString("Name"),
						Email = docSnap.GetString("email")
					};

					_onUserCompleteCallback?.Invoke(user);
				}
				else
				{
					throw new Exception("No such documment exists");
				}
			}
			else
			{
				throw new Exception("Failed to get documment");
			}
		}
	}
}
