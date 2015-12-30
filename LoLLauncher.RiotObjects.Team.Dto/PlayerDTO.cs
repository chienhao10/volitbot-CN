using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Team.Dto
{
	public class PlayerDTO : RiotGamesObject
	{
		public delegate void Callback(PlayerDTO result);

		private string type = "com.riotgames.team.dto.PlayerDTO";

		private PlayerDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("playerId")]
		public double PlayerId
		{
			get;
			set;
		}

		[InternalName("teamsSummary")]
		public List<object> TeamsSummary
		{
			get;
			set;
		}

		[InternalName("createdTeams")]
		public List<object> CreatedTeams
		{
			get;
			set;
		}

		[InternalName("playerTeams")]
		public List<object> PlayerTeams
		{
			get;
			set;
		}

		public PlayerDTO()
		{
		}

		public PlayerDTO(PlayerDTO.Callback callback)
		{
			this.callback = callback;
		}

		public PlayerDTO(TypedObject result)
		{
			base.SetFields<PlayerDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlayerDTO>(this, result);
			this.callback(this);
		}
	}
}
