using System;

namespace LoLLauncher
{
	public class StringValue : Attribute
	{
		private string _value;

		public string Value
		{
			get
			{
				return this._value;
			}
		}

		public StringValue(string value)
		{
			this._value = value;
		}
	}
}
