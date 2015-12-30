using System;

namespace LoLLauncher
{
	public class LocaleValue : Attribute
	{
		private string _value;

		public string Value
		{
			get
			{
				return this._value;
			}
		}

		public LocaleValue(string value)
		{
			this._value = value;
		}
	}
}
