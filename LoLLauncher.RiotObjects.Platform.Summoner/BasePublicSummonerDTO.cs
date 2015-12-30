using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class BasePublicSummonerDTO : RiotGamesObject
	{
		public delegate void Callback(BasePublicSummonerDTO result);

		private string type = "com.riotgames.platform.summoner.BasePublicSummonerDTO";

		private BasePublicSummonerDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("seasonTwoTier")]
		public string SeasonTwoTier
		{
			get;
			set;
		}

		[InternalName("internalName")]
		public string InternalName
		{
			get;
			set;
		}

		[InternalName("seasonOneTier")]
		public string SeasonOneTier
		{
			get;
			set;
		}

		[InternalName("acctId")]
		public double AcctId
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

		[InternalName("sumId")]
		public double SumId
		{
			get;
			set;
		}

		[InternalName("profileIconId")]
		public int ProfileIconId
		{
			get;
			set;
		}

		public BasePublicSummonerDTO()
		{
		}

		public BasePublicSummonerDTO(BasePublicSummonerDTO.Callback callback)
		{
			this.callback = callback;
		}

		public BasePublicSummonerDTO(TypedObject result)
		{
			base.SetFields<BasePublicSummonerDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<BasePublicSummonerDTO>(this, result);
			this.callback(this);
		}
	}
}
