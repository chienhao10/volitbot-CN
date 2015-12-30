using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class BotParticipant : Participant
	{
		public new delegate void Callback(BotParticipant result);

		private string type = "com.riotgames.platform.game.BotParticipant";

		private BotParticipant.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("botSkillLevel")]
		public int BotSkillLevel
		{
			get;
			set;
		}

		[InternalName("champion")]
		public ChampionDTO Champion
		{
			get;
			set;
		}

		[InternalName("botSkillLevelName")]
		public string BotSkillLevelName
		{
			get;
			set;
		}

		[InternalName("teamId")]
		public string TeamId
		{
			get;
			set;
		}

		[InternalName("isGameOwner")]
		public bool IsGameOwner
		{
			get;
			set;
		}

		[InternalName("pickMode")]
		public int PickMode
		{
			get;
			set;
		}

		[InternalName("team")]
		public int Team
		{
			get;
			set;
		}

		[InternalName("summonerInternalName")]
		public string SummonerInternalName
		{
			get;
			set;
		}

		[InternalName("pickTurn")]
		public int PickTurn
		{
			get;
			set;
		}

		[InternalName("badges")]
		public int Badges
		{
			get;
			set;
		}

		[InternalName("isMe")]
		public bool IsMe
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

		[InternalName("teamName")]
		public object TeamName
		{
			get;
			set;
		}

		public BotParticipant()
		{
		}

		public BotParticipant(BotParticipant.Callback callback)
		{
			this.callback = callback;
		}

		public BotParticipant(TypedObject result)
		{
			base.SetFields<BotParticipant>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<BotParticipant>(this, result);
			this.callback(this);
		}
	}
}
