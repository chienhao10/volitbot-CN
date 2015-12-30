using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class PlayerChampionSelectionDTO : RiotGamesObject
	{
		public delegate void Callback(PlayerChampionSelectionDTO result);

		private string type = "com.riotgames.platform.game.PlayerChampionSelectionDTO";

		private PlayerChampionSelectionDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("summonerInternalName")]
		public string SummonerInternalName
		{
			get;
			set;
		}

		[InternalName("spell2Id")]
		public double Spell2Id
		{
			get;
			set;
		}

		[InternalName("selectedSkinIndex")]
		public int SelectedSkinIndex
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

		[InternalName("spell1Id")]
		public double Spell1Id
		{
			get;
			set;
		}

		public PlayerChampionSelectionDTO()
		{
		}

		public PlayerChampionSelectionDTO(PlayerChampionSelectionDTO.Callback callback)
		{
			this.callback = callback;
		}

		public PlayerChampionSelectionDTO(TypedObject result)
		{
			base.SetFields<PlayerChampionSelectionDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlayerChampionSelectionDTO>(this, result);
			this.callback(this);
		}
	}
}
