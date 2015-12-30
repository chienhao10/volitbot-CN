using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class Participant : RiotGamesObject
	{
		public delegate void Callback(Participant result);

		private Participant.Callback callback;

		public Participant()
		{
		}

		public Participant(Participant.Callback callback)
		{
			this.callback = callback;
		}

		public Participant(TypedObject result)
		{
			base.SetFields<Participant>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<Participant>(this, result);
			this.callback(this);
		}
	}
}
