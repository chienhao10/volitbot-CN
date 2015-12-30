using System;

namespace LoLLauncher.RiotObjects.Team.Dto
{
	public class TeamMemberInfoDTO : RiotGamesObject
	{
		public delegate void Callback(TeamMemberInfoDTO result);

		private string type = "com.riotgames.team.dto.TeamMemberInfoDTO";

		private TeamMemberInfoDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("joinDate")]
		public DateTime JoinDate
		{
			get;
			set;
		}

		[InternalName("playerName")]
		public string PlayerName
		{
			get;
			set;
		}

		[InternalName("inviteDate")]
		public DateTime InviteDate
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

		[InternalName("playerId")]
		public double PlayerId
		{
			get;
			set;
		}

		public TeamMemberInfoDTO()
		{
		}

		public TeamMemberInfoDTO(TeamMemberInfoDTO.Callback callback)
		{
			this.callback = callback;
		}

		public TeamMemberInfoDTO(TypedObject result)
		{
			base.SetFields<TeamMemberInfoDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamMemberInfoDTO>(this, result);
			this.callback(this);
		}
	}
}
