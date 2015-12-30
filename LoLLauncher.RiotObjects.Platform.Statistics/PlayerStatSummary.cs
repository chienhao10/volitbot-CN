using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class PlayerStatSummary : RiotGamesObject
	{
		public delegate void Callback(PlayerStatSummary result);

		private string type = "com.riotgames.platform.statistics.PlayerStatSummary";

		private PlayerStatSummary.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("maxRating")]
		public int MaxRating
		{
			get;
			set;
		}

		[InternalName("playerStatSummaryTypeString")]
		public string PlayerStatSummaryTypeString
		{
			get;
			set;
		}

		[InternalName("aggregatedStats")]
		public SummaryAggStats AggregatedStats
		{
			get;
			set;
		}

		[InternalName("modifyDate")]
		public DateTime ModifyDate
		{
			get;
			set;
		}

		[InternalName("leaves")]
		public int Leaves
		{
			get;
			set;
		}

		[InternalName("playerStatSummaryType")]
		public string PlayerStatSummaryType
		{
			get;
			set;
		}

		[InternalName("userId")]
		public double UserId
		{
			get;
			set;
		}

		[InternalName("losses")]
		public int Losses
		{
			get;
			set;
		}

		[InternalName("rating")]
		public int Rating
		{
			get;
			set;
		}

		[InternalName("aggregatedStatsJson")]
		public object AggregatedStatsJson
		{
			get;
			set;
		}

		[InternalName("wins")]
		public int Wins
		{
			get;
			set;
		}

		public PlayerStatSummary()
		{
		}

		public PlayerStatSummary(PlayerStatSummary.Callback callback)
		{
			this.callback = callback;
		}

		public PlayerStatSummary(TypedObject result)
		{
			base.SetFields<PlayerStatSummary>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlayerStatSummary>(this, result);
			this.callback(this);
		}
	}
}
