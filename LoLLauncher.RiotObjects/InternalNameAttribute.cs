using System;

namespace LoLLauncher.RiotObjects
{
	public class InternalNameAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public InternalNameAttribute(string name)
		{
			this.Name = name;
		}
	}
}
