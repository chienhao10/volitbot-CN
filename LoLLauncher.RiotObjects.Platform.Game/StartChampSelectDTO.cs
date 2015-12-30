using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class StartChampSelectDTO : RiotGamesObject
	{
		public delegate void Callback(StartChampSelectDTO result);

		private string type = "com.riotgames.platform.game.StartChampSelectDTO";

		private StartChampSelectDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("invalidPlayers")]
		public List<object> InvalidPlayers
		{
			get;
			set;
		}

		public StartChampSelectDTO()
		{
		}

		public StartChampSelectDTO(StartChampSelectDTO.Callback callback)
		{
			this.callback = callback;
		}

		public StartChampSelectDTO(TypedObject result)
		{
			base.SetFields<StartChampSelectDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<StartChampSelectDTO>(this, result);
			this.callback(this);
		}
	}
}
