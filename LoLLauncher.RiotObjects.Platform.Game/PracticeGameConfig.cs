using LoLLauncher.RiotObjects.Platform.Game.Map;
using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class PracticeGameConfig : RiotGamesObject
	{
		public delegate void Callback(PracticeGameConfig result);

		private string type = "com.riotgames.platform.game.PracticeGameConfig";

		private PracticeGameConfig.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("passbackUrl")]
		public object PassbackUrl
		{
			get;
			set;
		}

		[InternalName("gameName")]
		public string GameName
		{
			get;
			set;
		}

		[InternalName("gameTypeConfig")]
		public int GameTypeConfig
		{
			get;
			set;
		}

		[InternalName("passbackDataPacket")]
		public object PassbackDataPacket
		{
			get;
			set;
		}

		[InternalName("gamePassword")]
		public string GamePassword
		{
			get;
			set;
		}

		[InternalName("gameMap")]
		public GameMap GameMap
		{
			get;
			set;
		}

		[InternalName("gameMode")]
		public string GameMode
		{
			get;
			set;
		}

		[InternalName("allowSpectators")]
		public string AllowSpectators
		{
			get;
			set;
		}

		[InternalName("maxNumPlayers")]
		public int MaxNumPlayers
		{
			get;
			set;
		}

		[InternalName("region")]
		public string Region
		{
			get;
			set;
		}

		public PracticeGameConfig()
		{
		}

		public PracticeGameConfig(PracticeGameConfig.Callback callback)
		{
			this.callback = callback;
		}

		public PracticeGameConfig(TypedObject result)
		{
			base.SetFields<PracticeGameConfig>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PracticeGameConfig>(this, result);
			this.callback(this);
		}
	}
}
