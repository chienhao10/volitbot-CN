using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class AllPublicSummonerDataDTO : RiotGamesObject
	{
		public delegate void Callback(AllPublicSummonerDataDTO result);

		private string type = "com.riotgames.platform.summoner.AllPublicSummonerDataDTO";

		private AllPublicSummonerDataDTO.Callback callback;

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
		public BasePublicSummonerDTO Summoner
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

		public AllPublicSummonerDataDTO()
		{
		}

		public AllPublicSummonerDataDTO(AllPublicSummonerDataDTO.Callback callback)
		{
			this.callback = callback;
		}

		public AllPublicSummonerDataDTO(TypedObject result)
		{
			base.SetFields<AllPublicSummonerDataDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AllPublicSummonerDataDTO>(this, result);
			this.callback(this);
		}
	}
}
