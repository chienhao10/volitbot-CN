using LoLLauncher.RiotObjects.Team;
using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics.Team
{
	public class TeamAggregatedStatsDTO : RiotGamesObject
	{
		public delegate void Callback(TeamAggregatedStatsDTO result);

		private string type = "com.riotgames.platform.statistics.team.TeamAggregatedStatsDTO";

		private TeamAggregatedStatsDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("queueType")]
		public string QueueType
		{
			get;
			set;
		}

		[InternalName("serializedToJson")]
		public string SerializedToJson
		{
			get;
			set;
		}

		[InternalName("playerAggregatedStatsList")]
		public List<TeamPlayerAggregatedStatsDTO> PlayerAggregatedStatsList
		{
			get;
			set;
		}

		[InternalName("teamId")]
		public TeamId TeamId
		{
			get;
			set;
		}

		public TeamAggregatedStatsDTO()
		{
		}

		public TeamAggregatedStatsDTO(TeamAggregatedStatsDTO.Callback callback)
		{
			this.callback = callback;
		}

		public TeamAggregatedStatsDTO(TypedObject result)
		{
			base.SetFields<TeamAggregatedStatsDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamAggregatedStatsDTO>(this, result);
			this.callback(this);
		}
	}
}
