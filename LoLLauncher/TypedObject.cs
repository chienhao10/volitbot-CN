using System;
using System.Collections.Generic;
using System.Text;

namespace LoLLauncher
{
	public class TypedObject : Dictionary<string, object>
	{
		private static long serialVersionUID = 1244827787088018807L;

		public string type;

		public TypedObject()
		{
			this.type = null;
		}

		public TypedObject(string type)
		{
			this.type = type;
		}

		public static TypedObject MakeArrayCollection(object[] data)
		{
			return new TypedObject("flex.messaging.io.ArrayCollection")
			{
				{
					"array",
					data
				}
			};
		}

		public TypedObject GetTO(string key)
		{
			if (base.ContainsKey(key) && base[key] is TypedObject)
			{
				return (TypedObject)base[key];
			}
			return null;
		}

		public string GetString(string key)
		{
			return (string)base[key];
		}

		public int? GetInt(string key)
		{
			object obj = base[key];
			if (obj == null)
			{
				return null;
			}
			if (obj is int)
			{
				return new int?((int)obj);
			}
			return new int?(Convert.ToInt32((double)obj));
		}

		public double? GetDouble(string key)
		{
			object obj = base[key];
			if (obj == null)
			{
				return null;
			}
			if (obj is double)
			{
				return new double?((double)obj);
			}
			return new double?(Convert.ToDouble((int)obj));
		}

		public bool GetBool(string key)
		{
			return (bool)base[key];
		}

		public object[] GetArray(string key)
		{
			if (base[key] is TypedObject && this.GetTO(key).type.Equals("flex.messaging.io.ArrayCollection"))
			{
				return (object[])this.GetTO(key)["array"];
			}
			return (object[])base[key];
		}

		public override string ToString()
		{
			if (this.type == null)
			{
				return base.ToString();
			}
			if (this.type.Equals("flex.messaging.io.ArrayCollection"))
			{
				StringBuilder stringBuilder = new StringBuilder();
				object[] array = (object[])base["array"];
				stringBuilder.Append("ArrayCollection[");
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i]);
					if (i < array.Length - 1)
					{
						stringBuilder.Append(", ");
					}
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
			string text = "";
			foreach (KeyValuePair<string, object> current in this)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					current.Key,
					" : ",
					current.Value,
					"\n"
				});
			}
			return text + this.type + ":" + base.ToString();
		}
	}
}
