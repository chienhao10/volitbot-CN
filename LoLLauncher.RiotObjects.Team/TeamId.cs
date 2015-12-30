using System;

namespace LoLLauncher.RiotObjects.Team
{
	public class TeamId : RiotGamesObject
	{
		public delegate void Callback(TeamId result);

		private string type = "com.riotgames.team.TeamId";

		private TeamId.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("fullId")]
		public string FullId
		{
			get;
			set;
		}

		public TeamId()
		{
		}

		public TeamId(TeamId.Callback callback)
		{
			this.callback = callback;
		}

		public TeamId(TypedObject result)
		{
			base.SetFields<TeamId>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamId>(this, result);
			this.callback(this);
		}
	}
}
