using System;

namespace LoLLauncher.RiotObjects.Leagues.Pojo
{
	public class LeagueItemDTO : RiotGamesObject
	{
		public delegate void Callback(LeagueItemDTO result);

		private string type = "com.riotgames.leagues.pojo.LeagueItemDTO";

		private LeagueItemDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("previousDayLeaguePosition")]
		public int PreviousDayLeaguePosition
		{
			get;
			set;
		}

		[InternalName("timeLastDecayMessageShown")]
		public double TimeLastDecayMessageShown
		{
			get;
			set;
		}

		[InternalName("hotStreak")]
		public bool HotStreak
		{
			get;
			set;
		}

		[InternalName("leagueName")]
		public string LeagueName
		{
			get;
			set;
		}

		[InternalName("miniSeries")]
		public object MiniSeries
		{
			get;
			set;
		}

		[InternalName("tier")]
		public string Tier
		{
			get;
			set;
		}

		[InternalName("freshBlood")]
		public bool FreshBlood
		{
			get;
			set;
		}

		[InternalName("lastPlayed")]
		public double LastPlayed
		{
			get;
			set;
		}

		[InternalName("playerOrTeamId")]
		public string PlayerOrTeamId
		{
			get;
			set;
		}

		[InternalName("leaguePoints")]
		public int LeaguePoints
		{
			get;
			set;
		}

		[InternalName("inactive")]
		public bool Inactive
		{
			get;
			set;
		}

		[InternalName("rank")]
		public string Rank
		{
			get;
			set;
		}

		[InternalName("veteran")]
		public bool Veteran
		{
			get;
			set;
		}

		[InternalName("queueType")]
		public string QueueType
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

		[InternalName("timeUntilDecay")]
		public double TimeUntilDecay
		{
			get;
			set;
		}

		[InternalName("playerOrTeamName")]
		public string PlayerOrTeamName
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

		public LeagueItemDTO()
		{
		}

		public LeagueItemDTO(LeagueItemDTO.Callback callback)
		{
			this.callback = callback;
		}

		public LeagueItemDTO(TypedObject result)
		{
			base.SetFields<LeagueItemDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<LeagueItemDTO>(this, result);
			this.callback(this);
		}
	}
}
