using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class PlayerParticipantStatsSummary : RiotGamesObject
	{
		public delegate void Callback(PlayerParticipantStatsSummary result);

		private string type = "com.riotgames.platform.statistics.PlayerParticipantStatsSummary";

		private PlayerParticipantStatsSummary.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("skinName")]
		public string SkinName
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

		[InternalName("profileIconId")]
		public int ProfileIconId
		{
			get;
			set;
		}

		[InternalName("elo")]
		public int Elo
		{
			get;
			set;
		}

		[InternalName("leaver")]
		public bool Leaver
		{
			get;
			set;
		}

		[InternalName("leaves")]
		public double Leaves
		{
			get;
			set;
		}

		[InternalName("teamId")]
		public double TeamId
		{
			get;
			set;
		}

		[InternalName("eloChange")]
		public int EloChange
		{
			get;
			set;
		}

		[InternalName("statistics")]
		public List<RawStatDTO> Statistics
		{
			get;
			set;
		}

		[InternalName("level")]
		public double Level
		{
			get;
			set;
		}

		[InternalName("botPlayer")]
		public bool BotPlayer
		{
			get;
			set;
		}

		[InternalName("userId")]
		public double UserId
		{
			get;
			set;
		}

		[InternalName("spell2Id")]
		public double Spell2Id
		{
			get;
			set;
		}

		[InternalName("losses")]
		public double Losses
		{
			get;
			set;
		}

		[InternalName("summonerName")]
		public string SummonerName
		{
			get;
			set;
		}

		[InternalName("wins")]
		public double Wins
		{
			get;
			set;
		}

		[InternalName("spell1Id")]
		public double Spell1Id
		{
			get;
			set;
		}

		public PlayerParticipantStatsSummary()
		{
		}

		public PlayerParticipantStatsSummary(PlayerParticipantStatsSummary.Callback callback)
		{
			this.callback = callback;
		}

		public PlayerParticipantStatsSummary(TypedObject result)
		{
			base.SetFields<PlayerParticipantStatsSummary>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlayerParticipantStatsSummary>(this, result);
			this.callback(this);
		}
	}
}
