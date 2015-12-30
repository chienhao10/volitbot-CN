using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class AggregatedStat : RiotGamesObject
	{
		public delegate void Callback(AggregatedStat result);

		private string type = "com.riotgames.platform.statistics.AggregatedStat";

		private AggregatedStat.Callback callback;

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

		[InternalName("championId")]
		public int ChampionId
		{
			get;
			set;
		}

		public AggregatedStat()
		{
		}

		public AggregatedStat(AggregatedStat.Callback callback)
		{
			this.callback = callback;
		}

		public AggregatedStat(TypedObject result)
		{
			base.SetFields<AggregatedStat>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AggregatedStat>(this, result);
			this.callback(this);
		}
	}
}
