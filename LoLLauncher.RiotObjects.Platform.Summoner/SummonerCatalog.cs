using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class SummonerCatalog : RiotGamesObject
	{
		public delegate void Callback(SummonerCatalog result);

		private string type = "com.riotgames.platform.summoner.SummonerCatalog";

		private SummonerCatalog.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("items")]
		public object Items
		{
			get;
			set;
		}

		[InternalName("talentTree")]
		public List<TalentGroup> TalentTree
		{
			get;
			set;
		}

		[InternalName("spellBookConfig")]
		public List<RuneSlot> SpellBookConfig
		{
			get;
			set;
		}

		public SummonerCatalog()
		{
		}

		public SummonerCatalog(SummonerCatalog.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerCatalog(TypedObject result)
		{
			base.SetFields<SummonerCatalog>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerCatalog>(this, result);
			this.callback(this);
		}
	}
}
