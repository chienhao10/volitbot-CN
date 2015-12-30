using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Boost
{
	public class SummonerActiveBoostsDTO : RiotGamesObject
	{
		public delegate void Callback(SummonerActiveBoostsDTO result);

		private string type = "com.riotgames.platform.summoner.boost.SummonerActiveBoostsDTO";

		private SummonerActiveBoostsDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("xpBoostEndDate")]
		public double XpBoostEndDate
		{
			get;
			set;
		}

		[InternalName("xpBoostPerWinCount")]
		public int XpBoostPerWinCount
		{
			get;
			set;
		}

		[InternalName("xpLoyaltyBoost")]
		public int XpLoyaltyBoost
		{
			get;
			set;
		}

		[InternalName("ipBoostPerWinCount")]
		public int IpBoostPerWinCount
		{
			get;
			set;
		}

		[InternalName("ipLoyaltyBoost")]
		public int IpLoyaltyBoost
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

		[InternalName("ipBoostEndDate")]
		public double IpBoostEndDate
		{
			get;
			set;
		}

		public SummonerActiveBoostsDTO()
		{
		}

		public SummonerActiveBoostsDTO(SummonerActiveBoostsDTO.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerActiveBoostsDTO(TypedObject result)
		{
			base.SetFields<SummonerActiveBoostsDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerActiveBoostsDTO>(this, result);
			this.callback(this);
		}
	}
}
