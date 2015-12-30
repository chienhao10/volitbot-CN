using System;
using System.Reflection;

namespace LoLLauncher
{
	public static class RegionInfo
	{
		public static string GetServerValue(Enum value)
		{
			string result = null;
			Type type = value.GetType();
			FieldInfo field = type.GetField(value.ToString());
			ServerValue[] array = field.GetCustomAttributes(typeof(ServerValue), false) as ServerValue[];
			if (array.Length > 0)
			{
				result = array[0].Value;
			}
			return result;
		}

		public static string GetLoginQueueValue(Enum value)
		{
			string result = null;
			Type type = value.GetType();
			FieldInfo field = type.GetField(value.ToString());
			LoginQueueValue[] array = field.GetCustomAttributes(typeof(LoginQueueValue), false) as LoginQueueValue[];
			if (array.Length > 0)
			{
				result = array[0].Value;
			}
			return result;
		}

		public static string GetLocaleValue(Enum value)
		{
			string result = null;
			Type type = value.GetType();
			FieldInfo field = type.GetField(value.ToString());
			LocaleValue[] array = field.GetCustomAttributes(typeof(LocaleValue), false) as LocaleValue[];
			if (array.Length > 0)
			{
				result = array[0].Value;
			}
			return result;
		}

		public static bool GetUseGarenaValue(Enum value)
		{
			bool result = false;
			Type type = value.GetType();
			FieldInfo field = type.GetField(value.ToString());
			UseGarenaValue[] array = field.GetCustomAttributes(typeof(UseGarenaValue), false) as UseGarenaValue[];
			if (array.Length > 0)
			{
				result = array[0].Value;
			}
			return result;
		}
	}
}
