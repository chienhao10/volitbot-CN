using System;

namespace LoLLauncher.RiotObjects
{
	public class UnclassedObject : RiotGamesObject
	{
		public delegate void Callback(TypedObject result);

		private string type = "";

		private UnclassedObject.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		public UnclassedObject(UnclassedObject.Callback callback)
		{
			this.callback = callback;
		}

		public override void DoCallback(TypedObject result)
		{
			this.callback(result);
		}
	}
}
