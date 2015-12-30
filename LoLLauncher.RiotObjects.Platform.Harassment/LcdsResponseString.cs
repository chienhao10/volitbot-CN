using System;

namespace LoLLauncher.RiotObjects.Platform.Harassment
{
	public class LcdsResponseString : RiotGamesObject
	{
		public delegate void Callback(LcdsResponseString result);

		private string type = "com.riotgames.platform.harassment.LcdsResponseString";

		private LcdsResponseString.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("value")]
		public string Value
		{
			get;
			set;
		}

		public LcdsResponseString()
		{
		}

		public LcdsResponseString(LcdsResponseString.Callback callback)
		{
			this.callback = callback;
		}

		public LcdsResponseString(TypedObject result)
		{
			base.SetFields<LcdsResponseString>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<LcdsResponseString>(this, result);
			this.callback(this);
		}
	}
}
