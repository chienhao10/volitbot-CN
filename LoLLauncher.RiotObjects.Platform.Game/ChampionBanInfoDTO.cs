using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class ChampionBanInfoDTO : RiotGamesObject
	{
		public delegate void Callback(ChampionBanInfoDTO result);

		private string type = "com.riotgames.platform.game.ChampionBanInfoDTO";

		private ChampionBanInfoDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("enemyOwned")]
		public bool EnemyOwned
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

		[InternalName("owned")]
		public bool Owned
		{
			get;
			set;
		}

		public ChampionBanInfoDTO(ChampionBanInfoDTO.Callback callback)
		{
			this.callback = callback;
		}

		public ChampionBanInfoDTO(TypedObject result)
		{
			base.SetFields<ChampionBanInfoDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<ChampionBanInfoDTO>(this, result);
			this.callback(this);
		}
	}
}
