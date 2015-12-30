using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class ChampionStatInfo : RiotGamesObject
	{
		public delegate void Callback(ChampionStatInfo result);

		private string type = "com.riotgames.platform.statistics.ChampionStatInfo";

		private ChampionStatInfo.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("totalGamesPlayed")]
		public int TotalGamesPlayed
		{
			get;
			set;
		}

		[InternalName("accountId")]
		public double AccountId
		{
			get;
			set;
		}

		[InternalName("stats")]
		public List<AggregatedStat> Stats
		{
			get;
			set;
		}

		[InternalName("championId")]
		public double ChampionId
		{
			get;
			set;
		}

		public ChampionStatInfo()
		{
		}

		public ChampionStatInfo(ChampionStatInfo.Callback callback)
		{
			this.callback = callback;
		}

		public ChampionStatInfo(TypedObject result)
		{
			base.SetFields<ChampionStatInfo>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<ChampionStatInfo>(this, result);
			this.callback(this);
		}
	}
}
