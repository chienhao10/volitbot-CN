using LoLLauncher.RiotObjects.Team.Stats;
using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Team.Dto
{
	public class TeamDTO : RiotGamesObject
	{
		public delegate void Callback(TeamDTO result);

		private string type = "com.riotgames.team.dto.TeamDTO";

		private TeamDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("teamStatSummary")]
		public TeamStatSummary TeamStatSummary
		{
			get;
			set;
		}

		[InternalName("status")]
		public string Status
		{
			get;
			set;
		}

		[InternalName("tag")]
		public string Tag
		{
			get;
			set;
		}

		[InternalName("roster")]
		public RosterDTO Roster
		{
			get;
			set;
		}

		[InternalName("lastGameDate")]
		public object LastGameDate
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

		[InternalName("messageOfDay")]
		public object MessageOfDay
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

		[InternalName("lastJoinDate")]
		public DateTime LastJoinDate
		{
			get;
			set;
		}

		[InternalName("secondLastJoinDate")]
		public DateTime SecondLastJoinDate
		{
			get;
			set;
		}

		[InternalName("secondsUntilEligibleForDeletion")]
		public double SecondsUntilEligibleForDeletion
		{
			get;
			set;
		}

		[InternalName("matchHistory")]
		public List<object> MatchHistory
		{
			get;
			set;
		}

		[InternalName("name")]
		public string Name
		{
			get;
			set;
		}

		[InternalName("thirdLastJoinDate")]
		public DateTime ThirdLastJoinDate
		{
			get;
			set;
		}

		[InternalName("createDate")]
		public DateTime CreateDate
		{
			get;
			set;
		}

		public TeamDTO()
		{
		}

		public TeamDTO(TeamDTO.Callback callback)
		{
			this.callback = callback;
		}

		public TeamDTO(TypedObject result)
		{
			base.SetFields<TeamDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamDTO>(this, result);
			this.callback(this);
		}
	}
}
