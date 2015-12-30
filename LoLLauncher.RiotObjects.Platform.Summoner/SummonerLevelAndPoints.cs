using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class SummonerLevelAndPoints : RiotGamesObject
	{
		public delegate void Callback(SummonerLevelAndPoints result);

		private string type = "com.riotgames.platform.summoner.SummonerLevelAndPoints";

		private SummonerLevelAndPoints.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("infPoints")]
		public double InfPoints
		{
			get;
			set;
		}

		[InternalName("expPoints")]
		public double ExpPoints
		{
			get;
			set;
		}

		[InternalName("summonerLevel")]
		public double SummonerLevel
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

		public SummonerLevelAndPoints()
		{
		}

		public SummonerLevelAndPoints(SummonerLevelAndPoints.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerLevelAndPoints(TypedObject result)
		{
			base.SetFields<SummonerLevelAndPoints>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerLevelAndPoints>(this, result);
			this.callback(this);
		}
	}
}
