using System;

namespace LoLLauncher
{
	public class ServerValue : Attribute
	{
		private string _value;

		public string Value
		{
			get
			{
				return this._value;
			}
		}

		public ServerValue(string value)
		{
			this._value = value;
		}
	}
}
