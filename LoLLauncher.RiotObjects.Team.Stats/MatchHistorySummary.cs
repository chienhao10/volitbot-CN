using System;

namespace LoLLauncher.RiotObjects.Team.Stats
{
	public class MatchHistorySummary : RiotGamesObject
	{
		public delegate void Callback(MatchHistorySummary result);

		private string type = "com.riotgames.team.stats.MatchHistorySummary";

		private MatchHistorySummary.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("gameMode")]
		public string GameMode
		{
			get;
			set;
		}

		[InternalName("mapId")]
		public int MapId
		{
			get;
			set;
		}

		[InternalName("assists")]
		public int Assists
		{
			get;
			set;
		}

		[InternalName("opposingTeamName")]
		public string OpposingTeamName
		{
			get;
			set;
		}

		[InternalName("invalid")]
		public bool Invalid
		{
			get;
			set;
		}

		[InternalName("deaths")]
		public int Deaths
		{
			get;
			set;
		}

		[InternalName("gameId")]
		public double GameId
		{
			get;
			set;
		}

		[InternalName("kills")]
		public int Kills
		{
			get;
			set;
		}

		[InternalName("win")]
		public bool Win
		{
			get;
			set;
		}

		[InternalName("date")]
		public double Date
		{
			get;
			set;
		}

		[InternalName("opposingTeamKills")]
		public int OpposingTeamKills
		{
			get;
			set;
		}

		public MatchHistorySummary()
		{
		}

		public MatchHistorySummary(MatchHistorySummary.Callback callback)
		{
			this.callback = callback;
		}

		public MatchHistorySummary(TypedObject result)
		{
			base.SetFields<MatchHistorySummary>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<MatchHistorySummary>(this, result);
			this.callback(this);
		}
	}
}
