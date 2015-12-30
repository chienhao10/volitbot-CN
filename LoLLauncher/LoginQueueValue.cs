using System;

namespace LoLLauncher
{
	public class LoginQueueValue : Attribute
	{
		private string _value;

		public string Value
		{
			get
			{
				return this._value;
			}
		}

		public LoginQueueValue(string value)
		{
			this._value = value;
		}
	}
}
