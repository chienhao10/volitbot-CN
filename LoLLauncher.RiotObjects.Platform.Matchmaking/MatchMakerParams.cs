using System;

namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
	public class MatchMakerParams : RiotGamesObject
	{
		public delegate void Callback(MatchMakerParams result);

		private string type = "com.riotgames.platform.matchmaking.MatchMakerParams";

		private MatchMakerParams.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("lastMaestroMessage")]
		public object LastMaestroMessage
		{
			get;
			set;
		}

		[InternalName("teamId")]
		public object TeamId
		{
			get;
			set;
		}

		[InternalName("languages")]
		public object Languages
		{
			get;
			set;
		}

		[InternalName("botDifficulty")]
		public string BotDifficulty
		{
			get;
			set;
		}

		[InternalName("team")]
		public object Team
		{
			get;
			set;
		}

		[InternalName("queueIds")]
		public int[] QueueIds
		{
			get;
			set;
		}

		[InternalName("invitationId")]
		public object InvitationId
		{
			get;
			set;
		}

		public MatchMakerParams()
		{
		}

		public MatchMakerParams(MatchMakerParams.Callback callback)
		{
			this.callback = callback;
		}

		public MatchMakerParams(TypedObject result)
		{
			base.SetFields<MatchMakerParams>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<MatchMakerParams>(this, result);
			this.callback(this);
		}
	}
}
