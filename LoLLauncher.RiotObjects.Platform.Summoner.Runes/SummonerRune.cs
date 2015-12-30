using LoLLauncher.RiotObjects.Platform.Catalog.Runes;
using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Runes
{
	public class SummonerRune : RiotGamesObject
	{
		public delegate void Callback(SummonerRune result);

		private string type = "com.riotgames.platform.summoner.runes.SummonerRune";

		private SummonerRune.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("purchased")]
		public DateTime Purchased
		{
			get;
			set;
		}

		[InternalName("purchaseDate")]
		public DateTime PurchaseDate
		{
			get;
			set;
		}

		[InternalName("runeId")]
		public int RuneId
		{
			get;
			set;
		}

		[InternalName("quantity")]
		public int Quantity
		{
			get;
			set;
		}

		[InternalName("rune")]
		public Rune Rune
		{
			get;
			set;
		}

		[InternalName("summonerId")]
		public double SummonerId
		{
			get;
			set;
		}

		public SummonerRune()
		{
		}

		public SummonerRune(SummonerRune.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerRune(TypedObject result)
		{
			base.SetFields<SummonerRune>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerRune>(this, result);
			this.callback(this);
		}
	}
}
