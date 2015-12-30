using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class PublicSummoner : RiotGamesObject
	{
		public delegate void Callback(PublicSummoner result);

		private string type = "com.riotgames.platform.summoner.PublicSummoner";

		private PublicSummoner.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("internalName")]
		public string InternalName
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

		[InternalName("profileIconId")]
		public int ProfileIconId
		{
			get;
			set;
		}

		[InternalName("revisionDate")]
		public DateTime RevisionDate
		{
			get;
			set;
		}

		[InternalName("revisionId")]
		public double RevisionId
		{
			get;
			set;
		}

		[InternalName("summonerLevel")]
		public double SummonerLevel
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

		public PublicSummoner()
		{
		}

		public PublicSummoner(PublicSummoner.Callback callback)
		{
			this.callback = callback;
		}

		public PublicSummoner(TypedObject result)
		{
			base.SetFields<PublicSummoner>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PublicSummoner>(this, result);
			this.callback(this);
		}
	}
}
