using LoLLauncher.RiotObjects.Platform.Catalog.Runes;
using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class RuneSlot : RiotGamesObject
	{
		public delegate void Callback(RuneSlot result);

		private string type = "com.riotgames.platform.summoner.RuneSlot";

		private RuneSlot.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("id")]
		public int Id
		{
			get;
			set;
		}

		[InternalName("minLevel")]
		public int MinLevel
		{
			get;
			set;
		}

		[InternalName("runeType")]
		public RuneType RuneType
		{
			get;
			set;
		}

		public RuneSlot()
		{
		}

		public RuneSlot(RuneSlot.Callback callback)
		{
			this.callback = callback;
		}

		public RuneSlot(TypedObject result)
		{
			base.SetFields<RuneSlot>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<RuneSlot>(this, result);
			this.callback(this);
		}
	}
}
