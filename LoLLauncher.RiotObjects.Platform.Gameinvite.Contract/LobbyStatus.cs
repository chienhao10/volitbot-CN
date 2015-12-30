using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Gameinvite.Contract
{
	public class LobbyStatus : RiotGamesObject
	{
		public delegate void Callback(LobbyStatus result);

		private string type = "com.riotgames.platform.gameinvite.contract.LobbyStatus";

		private LobbyStatus.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("gameMetaData")]
		public Dictionary<string, object> GameMetaData
		{
			get;
			set;
		}

		public LobbyStatus(LobbyStatus.Callback callback)
		{
			this.callback = callback;
		}

		public LobbyStatus(TypedObject result)
		{
			base.SetFields<LobbyStatus>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<LobbyStatus>(this, result);
			this.callback(this);
		}
	}
}
