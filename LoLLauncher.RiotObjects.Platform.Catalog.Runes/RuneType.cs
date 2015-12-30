using System;

namespace LoLLauncher.RiotObjects.Platform.Catalog.Runes
{
	public class RuneType : RiotGamesObject
	{
		public delegate void Callback(RuneType result);

		private string type = "com.riotgames.platform.catalog.runes.RuneType";

		private RuneType.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("runeTypeId")]
		public int RuneTypeId
		{
			get;
			set;
		}

		[InternalName("name")]
		public string Name
		{
			get;
			set;
		}

		public RuneType()
		{
		}

		public RuneType(RuneType.Callback callback)
		{
			this.callback = callback;
		}

		public RuneType(TypedObject result)
		{
			base.SetFields<RuneType>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<RuneType>(this, result);
			this.callback(this);
		}
	}
}
