using System;

namespace LoLLauncher.RiotObjects.Team
{
	public class TeamInfo : RiotGamesObject
	{
		public delegate void Callback(TeamInfo result);

		private string type = "com.riotgames.team.TeamInfo";

		private TeamInfo.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("secondsUntilEligibleForDeletion")]
		public double SecondsUntilEligibleForDeletion
		{
			get;
			set;
		}

		[InternalName("memberStatusString")]
		public string MemberStatusString
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

		[InternalName("name")]
		public string Name
		{
			get;
			set;
		}

		[InternalName("memberStatus")]
		public string MemberStatus
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

		public TeamInfo()
		{
		}

		public TeamInfo(TeamInfo.Callback callback)
		{
			this.callback = callback;
		}

		public TeamInfo(TypedObject result)
		{
			base.SetFields<TeamInfo>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamInfo>(this, result);
			this.callback(this);
		}
	}
}
