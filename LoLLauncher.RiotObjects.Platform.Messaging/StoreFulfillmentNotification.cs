using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using System;

namespace LoLLauncher.RiotObjects.Platform.Messaging
{
	internal class StoreFulfillmentNotification : RiotGamesObject
	{
		public delegate void Callback(StoreFulfillmentNotification result);

		private string type = "com.riotgames.platform.reroll.pojo.StoreFulfillmentNotification";

		private StoreFulfillmentNotification.Callback callback;

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

		[InternalName("inventoryType")]
		public string InventoryType
		{
			get;
			set;
		}

		[InternalName("data")]
		public ChampionDTO Data
		{
			get;
			set;
		}

		public StoreFulfillmentNotification()
		{
		}

		public StoreFulfillmentNotification(StoreFulfillmentNotification.Callback callback)
		{
			this.callback = callback;
		}

		public StoreFulfillmentNotification(TypedObject result)
		{
			base.SetFields<StoreFulfillmentNotification>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<StoreFulfillmentNotification>(this, result);
			this.callback(this);
		}
	}
}
