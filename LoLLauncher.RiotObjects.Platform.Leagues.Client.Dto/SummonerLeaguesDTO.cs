using LoLLauncher.RiotObjects.Leagues.Pojo;
using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto
{
	public class SummonerLeaguesDTO : RiotGamesObject
	{
		public delegate void Callback(SummonerLeaguesDTO result);

		private string type = "com.riotgames.platform.leagues.client.dto.SummonerLeaguesDTO";

		private SummonerLeaguesDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("summonerLeagues")]
		public List<LeagueListDTO> SummonerLeagues
		{
			get;
			set;
		}

		public SummonerLeaguesDTO()
		{
		}

		public SummonerLeaguesDTO(SummonerLeaguesDTO.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerLeaguesDTO(TypedObject result)
		{
			base.SetFields<SummonerLeaguesDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerLeaguesDTO>(this, result);
			this.callback(this);
		}
	}
}
