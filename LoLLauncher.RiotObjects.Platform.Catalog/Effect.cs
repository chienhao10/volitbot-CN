using LoLLauncher.RiotObjects.Platform.Catalog.Runes;
using System;

namespace LoLLauncher.RiotObjects.Platform.Catalog
{
	public class Effect : RiotGamesObject
	{
		public delegate void Callback(Effect result);

		private string type = "com.riotgames.platform.catalog.Effect";

		private Effect.Callback callback;

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

		[InternalName("gameCode")]
		public string GameCode
		{
			get;
			set;
		}

		[InternalName("name")]
		public string Name
		{
			get;
			set;
		}

		[InternalName("categoryId")]
		public object CategoryId
		{
			get;
			set;
		}

		[InternalName("runeType")]
		public RuneType RuneType
		{
			get;
			set;
		}

		public Effect()
		{
		}

		public Effect(Effect.Callback callback)
		{
			this.callback = callback;
		}

		public Effect(TypedObject result)
		{
			base.SetFields<Effect>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<Effect>(this, result);
			this.callback(this);
		}
	}
}
