using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class AggregatedStats : RiotGamesObject
	{
		public delegate void Callback(AggregatedStats result);

		private string type = "com.riotgames.platform.statistics.AggregatedStats";

		private AggregatedStats.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("lifetimeStatistics")]
		public List<AggregatedStat> LifetimeStatistics
		{
			get;
			set;
		}

		[InternalName("modifyDate")]
		public object ModifyDate
		{
			get;
			set;
		}

		[InternalName("key")]
		public AggregatedStatsKey Key
		{
			get;
			set;
		}

		[InternalName("aggregatedStatsJson")]
		public string AggregatedStatsJson
		{
			get;
			set;
		}

		public AggregatedStats()
		{
		}

		public AggregatedStats(AggregatedStats.Callback callback)
		{
			this.callback = callback;
		}

		public AggregatedStats(TypedObject result)
		{
			base.SetFields<AggregatedStats>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AggregatedStats>(this, result);
			this.callback(this);
		}
	}
}
