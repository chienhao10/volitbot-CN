using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Runes
{
	public class SummonerRuneInventory : RiotGamesObject
	{
		public delegate void Callback(SummonerRuneInventory result);

		private string type = "com.riotgames.platform.summoner.runes.SummonerRuneInventory";

		private SummonerRuneInventory.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("summonerRunesJson")]
		public object SummonerRunesJson
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

		[InternalName("summonerRunes")]
		public List<SummonerRune> SummonerRunes
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

		public SummonerRuneInventory()
		{
		}

		public SummonerRuneInventory(SummonerRuneInventory.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerRuneInventory(TypedObject result)
		{
			base.SetFields<SummonerRuneInventory>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerRuneInventory>(this, result);
			this.callback(this);
		}
	}
}
