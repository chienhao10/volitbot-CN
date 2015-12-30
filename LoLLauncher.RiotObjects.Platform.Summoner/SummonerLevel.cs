using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class SummonerLevel : RiotGamesObject
	{
		public delegate void Callback(SummonerLevel result);

		private string type = "com.riotgames.platform.summoner.SummonerLevel";

		private SummonerLevel.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("expTierMod")]
		public double ExpTierMod
		{
			get;
			set;
		}

		[InternalName("grantRp")]
		public double GrantRp
		{
			get;
			set;
		}

		[InternalName("expForLoss")]
		public double ExpForLoss
		{
			get;
			set;
		}

		[InternalName("summonerTier")]
		public double SummonerTier
		{
			get;
			set;
		}

		[InternalName("infTierMod")]
		public double InfTierMod
		{
			get;
			set;
		}

		[InternalName("expToNextLevel")]
		public double ExpToNextLevel
		{
			get;
			set;
		}

		[InternalName("expForWin")]
		public double ExpForWin
		{
			get;
			set;
		}

		[InternalName("summonerLevel")]
		public double Level
		{
			get;
			set;
		}

		public SummonerLevel()
		{
		}

		public SummonerLevel(SummonerLevel.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerLevel(TypedObject result)
		{
			base.SetFields<SummonerLevel>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerLevel>(this, result);
			this.callback(this);
		}
	}
}
