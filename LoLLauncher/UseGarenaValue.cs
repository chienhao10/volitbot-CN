using System;

namespace LoLLauncher
{
	public class UseGarenaValue : Attribute
	{
		private bool _value;

		public bool Value
		{
			get
			{
				return this._value;
			}
		}

		public UseGarenaValue(bool value)
		{
			this._value = value;
		}
	}
}
