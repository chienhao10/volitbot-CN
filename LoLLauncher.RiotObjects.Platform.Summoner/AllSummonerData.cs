using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class AllSummonerData : RiotGamesObject
	{
		public delegate void Callback(AllSummonerData result);

		private string type = "com.riotgames.platform.summoner.AllSummonerData";

		private AllSummonerData.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("spellBook")]
		public SpellBookDTO SpellBook
		{
			get;
			set;
		}

		[InternalName("summonerDefaultSpells")]
		public SummonerDefaultSpells SummonerDefaultSpells
		{
			get;
			set;
		}

		[InternalName("summonerTalentsAndPoints")]
		public SummonerTalentsAndPoints SummonerTalentsAndPoints
		{
			get;
			set;
		}

		[InternalName("summoner")]
		public Summoner Summoner
		{
			get;
			set;
		}

		[InternalName("masteryBook")]
		public MasteryBookDTO MasteryBook
		{
			get;
			set;
		}

		[InternalName("summonerLevelAndPoints")]
		public SummonerLevelAndPoints SummonerLevelAndPoints
		{
			get;
			set;
		}

		[InternalName("summonerLevel")]
		public SummonerLevel SummonerLevel
		{
			get;
			set;
		}

		public AllSummonerData()
		{
		}

		public AllSummonerData(AllSummonerData.Callback callback)
		{
			this.callback = callback;
		}

		public AllSummonerData(TypedObject result)
		{
			base.SetFields<AllSummonerData>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AllSummonerData>(this, result);
			this.callback(this);
		}
	}
}
