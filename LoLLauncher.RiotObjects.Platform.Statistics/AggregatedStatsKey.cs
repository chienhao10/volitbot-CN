using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class AggregatedStatsKey : RiotGamesObject
	{
		public delegate void Callback(AggregatedStatsKey result);

		private string type = "com.riotgames.platform.statistics.AggregatedStatsKey";

		private AggregatedStatsKey.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("gameMode")]
		public string GameMode
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

		[InternalName("gameModeString")]
		public string GameModeString
		{
			get;
			set;
		}

		public AggregatedStatsKey()
		{
		}

		public AggregatedStatsKey(AggregatedStatsKey.Callback callback)
		{
			this.callback = callback;
		}

		public AggregatedStatsKey(TypedObject result)
		{
			base.SetFields<AggregatedStatsKey>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AggregatedStatsKey>(this, result);
			this.callback(this);
		}
	}
}
