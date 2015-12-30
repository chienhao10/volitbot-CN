using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Spellbook
{
	public class SlotEntry : RiotGamesObject
	{
		public delegate void Callback(SlotEntry result);

		private string type = "com.riotgames.platform.summoner.spellbook.SlotEntry";

		private SlotEntry.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("runeId")]
		public int RuneId
		{
			get;
			set;
		}

		[InternalName("runeSlotId")]
		public int RuneSlotId
		{
			get;
			set;
		}

		public SlotEntry()
		{
		}

		public SlotEntry(SlotEntry.Callback callback)
		{
			this.callback = callback;
		}

		public SlotEntry(TypedObject result)
		{
			base.SetFields<SlotEntry>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SlotEntry>(this, result);
			this.callback(this);
		}
	}
}
