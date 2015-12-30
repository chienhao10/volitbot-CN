using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics.Team
{
	public class TeamPlayerAggregatedStatsDTO : RiotGamesObject
	{
		public delegate void Callback(TeamPlayerAggregatedStatsDTO result);

		private string type = "com.riotgames.platform.statistics.team.TeamPlayerAggregatedStatsDTO";

		private TeamPlayerAggregatedStatsDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("playerId")]
		public double PlayerId
		{
			get;
			set;
		}

		[InternalName("aggregatedStats")]
		public AggregatedStats AggregatedStats
		{
			get;
			set;
		}

		public TeamPlayerAggregatedStatsDTO()
		{
		}

		public TeamPlayerAggregatedStatsDTO(TeamPlayerAggregatedStatsDTO.Callback callback)
		{
			this.callback = callback;
		}

		public TeamPlayerAggregatedStatsDTO(TypedObject result)
		{
			base.SetFields<TeamPlayerAggregatedStatsDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamPlayerAggregatedStatsDTO>(this, result);
			this.callback(this);
		}
	}
}
