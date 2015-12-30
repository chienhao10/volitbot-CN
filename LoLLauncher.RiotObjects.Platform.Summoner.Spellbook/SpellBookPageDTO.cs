using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Spellbook
{
	public class SpellBookPageDTO : RiotGamesObject
	{
		public delegate void Callback(SpellBookPageDTO result);

		private string type = "com.riotgames.platform.summoner.spellbook.SpellBookPageDTO";

		private SpellBookPageDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("slotEntries")]
		public List<SlotEntry> SlotEntries
		{
			get;
			set;
		}

		[InternalName("summonerId")]
		public int SummonerId
		{
			get;
			set;
		}

		[InternalName("createDate")]
		public DateTime CreateDate
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

		[InternalName("pageId")]
		public int PageId
		{
			get;
			set;
		}

		[InternalName("current")]
		public bool Current
		{
			get;
			set;
		}

		public SpellBookPageDTO()
		{
		}

		public SpellBookPageDTO(SpellBookPageDTO.Callback callback)
		{
			this.callback = callback;
		}

		public SpellBookPageDTO(TypedObject result)
		{
			base.SetFields<SpellBookPageDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SpellBookPageDTO>(this, result);
			this.callback(this);
		}
	}
}
