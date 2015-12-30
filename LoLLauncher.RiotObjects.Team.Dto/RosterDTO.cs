using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Team.Dto
{
	public class RosterDTO : RiotGamesObject
	{
		public delegate void Callback(RosterDTO result);

		private string type = "com.riotgames.team.dto.RosterDTO";

		private RosterDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("ownerId")]
		public double OwnerId
		{
			get;
			set;
		}

		[InternalName("memberList")]
		public List<TeamMemberInfoDTO> MemberList
		{
			get;
			set;
		}

		public RosterDTO()
		{
		}

		public RosterDTO(RosterDTO.Callback callback)
		{
			this.callback = callback;
		}

		public RosterDTO(TypedObject result)
		{
			base.SetFields<RosterDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<RosterDTO>(this, result);
			this.callback(this);
		}
	}
}
