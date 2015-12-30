using System;
using System.Collections.Generic;

namespace LoLLauncher
{
	public class ClassDefinition
	{
		public string type;

		public bool externalizable;

		public bool dynamic;

		public List<string> members = new List<string>();
	}
}
