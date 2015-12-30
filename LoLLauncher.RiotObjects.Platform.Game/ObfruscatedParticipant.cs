using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class ObfruscatedParticipant : Participant
	{
		public new delegate void Callback(ObfruscatedParticipant result);

		private string type = "com.riotgames.platform.game.ObfruscatedParticipant";

		private ObfruscatedParticipant.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("badges")]
		public int Badges
		{
			get;
			set;
		}

		[InternalName("index")]
		public int Index
		{
			get;
			set;
		}

		[InternalName("clientInSynch")]
		public bool ClientInSynch
		{
			get;
			set;
		}

		[InternalName("gameUniqueId")]
		public int GameUniqueId
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

		public ObfruscatedParticipant()
		{
		}

		public ObfruscatedParticipant(ObfruscatedParticipant.Callback callback)
		{
			this.callback = callback;
		}

		public ObfruscatedParticipant(TypedObject result)
		{
			base.SetFields<ObfruscatedParticipant>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<ObfruscatedParticipant>(this, result);
			this.callback(this);
		}
	}
}
