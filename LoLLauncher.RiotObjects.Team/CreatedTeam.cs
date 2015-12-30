using System;

namespace LoLLauncher.RiotObjects.Team
{
	public class CreatedTeam : RiotGamesObject
	{
		public delegate void Callback(CreatedTeam result);

		private string type = "com.riotgames.team.CreatedTeam";

		private CreatedTeam.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("timeStamp")]
		public double TimeStamp
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

		public CreatedTeam()
		{
		}

		public CreatedTeam(CreatedTeam.Callback callback)
		{
			this.callback = callback;
		}

		public CreatedTeam(TypedObject result)
		{
			base.SetFields<CreatedTeam>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<CreatedTeam>(this, result);
			this.callback(this);
		}
	}
}
