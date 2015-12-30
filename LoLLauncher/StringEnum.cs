using System;
using System.Reflection;

namespace LoLLauncher
{
	public static class StringEnum
	{
		public static string GetStringValue(Enum value)
		{
			string result = null;
			Type type = value.GetType();
			FieldInfo field = type.GetField(value.ToString());
			StringValue[] array = field.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
			if (array.Length > 0)
			{
				result = array[0].Value;
			}
			return result;
		}
	}
}
