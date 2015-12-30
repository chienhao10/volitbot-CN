using System;

namespace LoLLauncher.RiotObjects.Platform.Broadcast
{
	public class BroadcastNotification : RiotGamesObject
	{
		public delegate void Callback(BroadcastNotification result);

		private string type = "com.riotgames.platform.broadcast.BroadcastNotification";

		private BroadcastNotification.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		public BroadcastNotification()
		{
		}

		public BroadcastNotification(BroadcastNotification.Callback callback)
		{
			this.callback = callback;
		}

		public BroadcastNotification(TypedObject result)
		{
			base.SetFields<BroadcastNotification>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<BroadcastNotification>(this, result);
			this.callback(this);
		}
	}
}
