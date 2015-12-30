using LoLLauncher.RiotObjects.Platform.Summoner;
using System;

namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
	public class QueueDodger : RiotGamesObject
	{
		public delegate void Callback(QueueDodger result);

		private string type = "com.riotgames.platform.matchmaking.QueueDodger";

		private QueueDodger.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("reasonFailed")]
		public string ReasonFailed
		{
			get;
			set;
		}

		[InternalName("accessToken")]
		public string AccessToken
		{
			get;
			set;
		}

		[InternalName("summoner")]
		public Summoner Summoner
		{
			get;
			set;
		}

		[InternalName("dodgePenaltyRemainingTime")]
		public int DodgePenaltyRemainingTime
		{
			get;
			set;
		}

		[InternalName("leaverPenaltyMillisRemaining")]
		public int LeaverPenaltyMillisRemaining
		{
			get;
			set;
		}

		public QueueDodger()
		{
		}

		public QueueDodger(QueueDodger.Callback callback)
		{
			this.callback = callback;
		}

		public QueueDodger(TypedObject result)
		{
			base.SetFields<QueueDodger>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<QueueDodger>(this, result);
			this.callback(this);
		}
	}
}
