using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class SummaryAggStat : RiotGamesObject
	{
		public delegate void Callback(SummaryAggStat result);

		private string type = "com.riotgames.platform.statistics.SummaryAggStat";

		private SummaryAggStat.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("statType")]
		public string StatType
		{
			get;
			set;
		}

		[InternalName("count")]
		public double Count
		{
			get;
			set;
		}

		[InternalName("value")]
		public double Value
		{
			get;
			set;
		}

		public SummaryAggStat(SummaryAggStat.Callback callback)
		{
			this.callback = callback;
		}

		public SummaryAggStat(TypedObject result)
		{
			base.SetFields<SummaryAggStat>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummaryAggStat>(this, result);
			this.callback(this);
		}
	}
}
