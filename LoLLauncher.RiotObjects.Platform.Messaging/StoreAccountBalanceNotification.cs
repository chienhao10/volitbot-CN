using System;

namespace LoLLauncher.RiotObjects.Platform.Messaging
{
	internal class StoreAccountBalanceNotification : RiotGamesObject
	{
		public delegate void Callback(StoreAccountBalanceNotification result);

		private string type = "com.riotgames.platform.reroll.pojo.StoreAccountBalanceNotification";

		private StoreAccountBalanceNotification.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("rp")]
		public double Rp
		{
			get;
			set;
		}

		[InternalName("ip")]
		public double Ip
		{
			get;
			set;
		}

		public StoreAccountBalanceNotification()
		{
		}

		public StoreAccountBalanceNotification(StoreAccountBalanceNotification.Callback callback)
		{
			this.callback = callback;
		}

		public StoreAccountBalanceNotification(TypedObject result)
		{
			base.SetFields<StoreAccountBalanceNotification>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<StoreAccountBalanceNotification>(this, result);
			this.callback(this);
		}
	}
}
