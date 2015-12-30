using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class GameTypeConfigDTO : RiotGamesObject
	{
		public delegate void Callback(GameTypeConfigDTO result);

		private string type = "com.riotgames.platform.game.GameTypeConfigDTO";

		private GameTypeConfigDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("id")]
		public int Id
		{
			get;
			set;
		}

		[InternalName("allowTrades")]
		public bool AllowTrades
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

		[InternalName("mainPickTimerDuration")]
		public int MainPickTimerDuration
		{
			get;
			set;
		}

		[InternalName("exclusivePick")]
		public bool ExclusivePick
		{
			get;
			set;
		}

		[InternalName("pickMode")]
		public string PickMode
		{
			get;
			set;
		}

		[InternalName("maxAllowableBans")]
		public int MaxAllowableBans
		{
			get;
			set;
		}

		[InternalName("banTimerDuration")]
		public int BanTimerDuration
		{
			get;
			set;
		}

		[InternalName("postPickTimerDuration")]
		public int PostPickTimerDuration
		{
			get;
			set;
		}

		public GameTypeConfigDTO()
		{
		}

		public GameTypeConfigDTO(GameTypeConfigDTO.Callback callback)
		{
			this.callback = callback;
		}

		public GameTypeConfigDTO(TypedObject result)
		{
			base.SetFields<GameTypeConfigDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<GameTypeConfigDTO>(this, result);
			this.callback(this);
		}
	}
}
