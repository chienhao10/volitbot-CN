using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class SummaryAggStats : RiotGamesObject
	{
		public delegate void Callback(SummaryAggStats result);

		private string type = "com.riotgames.platform.statistics.SummaryAggStats";

		private SummaryAggStats.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("statsJson")]
		public object StatsJson
		{
			get;
			set;
		}

		[InternalName("stats")]
		public List<SummaryAggStat> Stats
		{
			get;
			set;
		}

		public SummaryAggStats()
		{
		}

		public SummaryAggStats(SummaryAggStats.Callback callback)
		{
			this.callback = callback;
		}

		public SummaryAggStats(TypedObject result)
		{
			base.SetFields<SummaryAggStats>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummaryAggStats>(this, result);
			this.callback(this);
		}
	}
}
