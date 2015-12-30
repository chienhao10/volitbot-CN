using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class PlayerStats : RiotGamesObject
	{
		public delegate void Callback(PlayerStats result);

		private string type = "com.riotgames.platform.statistics.PlayerStats";

		private PlayerStats.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("timeTrackedStats")]
		public List<TimeTrackedStat> TimeTrackedStats
		{
			get;
			set;
		}

		[InternalName("promoGamesPlayed")]
		public int PromoGamesPlayed
		{
			get;
			set;
		}

		[InternalName("promoGamesPlayedLastUpdated")]
		public object PromoGamesPlayedLastUpdated
		{
			get;
			set;
		}

		public PlayerStats()
		{
		}

		public PlayerStats(PlayerStats.Callback callback)
		{
			this.callback = callback;
		}

		public PlayerStats(TypedObject result)
		{
			base.SetFields<PlayerStats>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlayerStats>(this, result);
			this.callback(this);
		}
	}
}
