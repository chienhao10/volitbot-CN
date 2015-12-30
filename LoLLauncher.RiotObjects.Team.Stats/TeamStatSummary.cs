using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Team.Stats
{
	public class TeamStatSummary : RiotGamesObject
	{
		public delegate void Callback(TeamStatSummary result);

		private string type = "com.riotgames.team.stats.TeamStatSummary";

		private TeamStatSummary.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("teamStatDetails")]
		public List<TeamStatDetail> TeamStatDetails
		{
			get;
			set;
		}

		[InternalName("teamIdString")]
		public string TeamIdString
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

		public TeamStatSummary()
		{
		}

		public TeamStatSummary(TeamStatSummary.Callback callback)
		{
			this.callback = callback;
		}

		public TeamStatSummary(TypedObject result)
		{
			base.SetFields<TeamStatSummary>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamStatSummary>(this, result);
			this.callback(this);
		}
	}
}
