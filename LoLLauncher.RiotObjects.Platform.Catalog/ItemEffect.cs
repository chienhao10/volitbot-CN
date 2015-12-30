using System;

namespace LoLLauncher.RiotObjects.Platform.Catalog
{
	public class ItemEffect : RiotGamesObject
	{
		public delegate void Callback(ItemEffect result);

		private string type = "com.riotgames.platform.catalog.ItemEffect";

		private ItemEffect.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("effectId")]
		public int EffectId
		{
			get;
			set;
		}

		[InternalName("itemEffectId")]
		public int ItemEffectId
		{
			get;
			set;
		}

		[InternalName("effect")]
		public Effect Effect
		{
			get;
			set;
		}

		[InternalName("value")]
		public string Value
		{
			get;
			set;
		}

		[InternalName("itemId")]
		public int ItemId
		{
			get;
			set;
		}

		public ItemEffect()
		{
		}

		public ItemEffect(ItemEffect.Callback callback)
		{
			this.callback = callback;
		}

		public ItemEffect(TypedObject result)
		{
			base.SetFields<ItemEffect>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<ItemEffect>(this, result);
			this.callback(this);
		}
	}
}
