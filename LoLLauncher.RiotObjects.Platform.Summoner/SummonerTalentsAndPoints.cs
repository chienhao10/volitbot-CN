using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class SummonerTalentsAndPoints : RiotGamesObject
	{
		public delegate void Callback(SummonerTalentsAndPoints result);

		private string type = "com.riotgames.platform.summoner.SummonerTalentsAndPoints";

		private SummonerTalentsAndPoints.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("talentPoints")]
		public int TalentPoints
		{
			get;
			set;
		}

		[InternalName("modifyDate")]
		public DateTime ModifyDate
		{
			get;
			set;
		}

		[InternalName("createDate")]
		public DateTime CreateDate
		{
			get;
			set;
		}

		[InternalName("summonerId")]
		public double SummonerId
		{
			get;
			set;
		}

		public SummonerTalentsAndPoints()
		{
		}

		public SummonerTalentsAndPoints(SummonerTalentsAndPoints.Callback callback)
		{
			this.callback = callback;
		}

		public SummonerTalentsAndPoints(TypedObject result)
		{
			base.SetFields<SummonerTalentsAndPoints>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SummonerTalentsAndPoints>(this, result);
			this.callback(this);
		}
	}
}
