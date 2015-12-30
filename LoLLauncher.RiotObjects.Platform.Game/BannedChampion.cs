using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class BannedChampion : RiotGamesObject
	{
		public delegate void Callback(BannedChampion result);

		private string type = "com.riotgames.platform.game.BannedChampion";

		private BannedChampion.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("pickTurn")]
		public int PickTurn
		{
			get;
			set;
		}

		[InternalName("championId")]
		public int ChampionId
		{
			get;
			set;
		}

		[InternalName("teamId")]
		public int TeamId
		{
			get;
			set;
		}

		public BannedChampion(BannedChampion.Callback callback)
		{
			this.callback = callback;
		}

		public BannedChampion(TypedObject result)
		{
			base.SetFields<BannedChampion>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<BannedChampion>(this, result);
			this.callback(this);
		}
	}
}
