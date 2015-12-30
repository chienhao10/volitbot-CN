using LoLLauncher.RiotObjects.Leagues.Pojo;
using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto
{
	public class SummonerLeagueItemsDTO : RiotGamesObject
	{
		public delegate void Callback(SummonerLeagueItemsDTO result);

		private string type = "com.riotgames.platform.leagues.client.dto.SummonerLeagueItemsDTO";

		private SummonerLeagueItemsDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("summonerLeagues")]
		public List<LeagueItemDTO> SummonerLeagues
		{
			get;
			set;
		}

		public SummonerLeagueItemsDTO()
		{
		}

		public SummonerLeagueItemsDTO(SummonerLeagueItemsDTO.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerLeagueItemsDTO(TypedObject result)
		{
			base.SetFields<SummonerLeagueItemsDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerLeagueItemsDTO>(this, result);
			this.callback(this);
		}
	}
}
