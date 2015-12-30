using System;

namespace LoLLauncher.RiotObjects.Platform.Game.Message
{
	public class GameNotification : RiotGamesObject
	{
		public delegate void Callback(GameNotification result);

		private string type = "com.riotgames.platform.game.message.GameNotification";

		private GameNotification.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("messageCode")]
		public string MessageCode
		{
			get;
			set;
		}

		[InternalName("type")]
		public string Type
		{
			get;
			set;
		}

		[InternalName("messageArgument")]
		public object MessageArgument
		{
			get;
			set;
		}

		public GameNotification()
		{
		}

		public GameNotification(GameNotification.Callback callback)
		{
			this.callback = callback;
		}

		public GameNotification(TypedObject result)
		{
			base.SetFields<GameNotification>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<GameNotification>(this, result);
			this.callback(this);
		}
	}
}
