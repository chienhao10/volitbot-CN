using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class RecentGames : RiotGamesObject
	{
		public delegate void Callback(RecentGames result);

		private string type = "com.riotgames.platform.statistics.RecentGames";

		private RecentGames.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("recentGamesJson")]
		public object RecentGamesJson
		{
			get;
			set;
		}

		[InternalName("playerGameStatsMap")]
		public TypedObject PlayerGameStatsMap
		{
			get;
			set;
		}

		[InternalName("gameStatistics")]
		public List<PlayerGameStats> GameStatistics
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

		public RecentGames()
		{
		}

		public RecentGames(RecentGames.Callback callback)
		{
			this.callback = callback;
		}

		public RecentGames(TypedObject result)
		{
			base.SetFields<RecentGames>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<RecentGames>(this, result);
			this.callback(this);
		}
	}
}
