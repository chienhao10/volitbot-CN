using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Leagues.Pojo
{
	public class LeagueListDTO : RiotGamesObject
	{
		public delegate void Callback(LeagueListDTO result);

		private string type = "com.riotgames.leagues.pojo.LeagueListDTO";

		private LeagueListDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("queue")]
		public string Queue
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

		[InternalName("tier")]
		public string Tier
		{
			get;
			set;
		}

		[InternalName("requestorsRank")]
		public string RequestorsRank
		{
			get;
			set;
		}

		[InternalName("entries")]
		public List<LeagueItemDTO> Entries
		{
			get;
			set;
		}

		[InternalName("requestorsName")]
		public string RequestorsName
		{
			get;
			set;
		}

		public LeagueListDTO()
		{
		}

		public LeagueListDTO(LeagueListDTO.Callback callback)
		{
			this.callback = callback;
		}

		public LeagueListDTO(TypedObject result)
		{
			base.SetFields<LeagueListDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<LeagueListDTO>(this, result);
			this.callback(this);
		}
	}
}
