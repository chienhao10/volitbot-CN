using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Spellbook
{
	public class SpellBookDTO : RiotGamesObject
	{
		public delegate void Callback(SpellBookDTO result);

		private string type = "com.riotgames.platform.summoner.spellbook.SpellBookDTO";

		private SpellBookDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("bookPagesJson")]
		public object BookPagesJson
		{
			get;
			set;
		}

		[InternalName("bookPages")]
		public List<SpellBookPageDTO> BookPages
		{
			get;
			set;
		}

		[InternalName("dateString")]
		public string DateString
		{
			get;
			set;
		}

		[InternalName("summonerId")]
		public double SummonerId
		{
			get;
			set;
		}

		public SpellBookDTO()
		{
		}

		public SpellBookDTO(SpellBookDTO.Callback callback)
		{
			this.callback = callback;
		}

		public SpellBookDTO(TypedObject result)
		{
			base.SetFields<SpellBookDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SpellBookDTO>(this, result);
			this.callback(this);
		}
	}
}
