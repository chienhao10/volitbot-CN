using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class SummonerDefaultSpells : RiotGamesObject
	{
		public delegate void Callback(SummonerDefaultSpells result);

		private string type = "com.riotgames.platform.summoner.SummonerDefaultSpells";

		private SummonerDefaultSpells.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("summonerDefaultSpellsJson")]
		public object SummonerDefaultSpellsJson
		{
			get;
			set;
		}

		[InternalName("summonerDefaultSpellMap")]
		public TypedObject SummonerDefaultSpellMap
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

		public SummonerDefaultSpells()
		{
		}

		public SummonerDefaultSpells(SummonerDefaultSpells.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerDefaultSpells(TypedObject result)
		{
			base.SetFields<SummonerDefaultSpells>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerDefaultSpells>(this, result);
			this.callback(this);
		}
	}
}
