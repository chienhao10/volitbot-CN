using System;

namespace LoLLauncher.RiotObjects.Team.Stats
{
	public class TeamStatDetail : RiotGamesObject
	{
		public delegate void Callback(TeamStatDetail result);

		private string type = "com.riotgames.team.stats.TeamStatDetail";

		private TeamStatDetail.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("maxRating")]
		public int MaxRating
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

		[InternalName("seedRating")]
		public int SeedRating
		{
			get;
			set;
		}

		[InternalName("losses")]
		public int Losses
		{
			get;
			set;
		}

		[InternalName("rating")]
		public int Rating
		{
			get;
			set;
		}

		[InternalName("teamStatTypeString")]
		public string TeamStatTypeString
		{
			get;
			set;
		}

		[InternalName("averageGamesPlayed")]
		public int AverageGamesPlayed
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

		[InternalName("wins")]
		public int Wins
		{
			get;
			set;
		}

		[InternalName("teamStatType")]
		public string TeamStatType
		{
			get;
			set;
		}

		public TeamStatDetail()
		{
		}

		public TeamStatDetail(TeamStatDetail.Callback callback)
		{
			this.callback = callback;
		}

		public TeamStatDetail(TypedObject result)
		{
			base.SetFields<TeamStatDetail>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TeamStatDetail>(this, result);
			this.callback(this);
		}
	}
}
