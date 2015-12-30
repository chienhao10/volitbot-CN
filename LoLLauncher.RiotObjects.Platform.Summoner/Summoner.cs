using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class Summoner : RiotGamesObject
	{
		public delegate void Callback(Summoner result);

		private string type = "com.riotgames.platform.summoner.Summoner";

		private Summoner.Callback callback;

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

		[InternalName("acctId")]
		public double AcctId
		{
			get;
			set;
		}

		[InternalName("helpFlag")]
		public bool HelpFlag
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

		[InternalName("displayEloQuestionaire")]
		public bool DisplayEloQuestionaire
		{
			get;
			set;
		}

		[InternalName("lastGameDate")]
		public DateTime LastGameDate
		{
			get;
			set;
		}

		[InternalName("advancedTutorialFlag")]
		public bool AdvancedTutorialFlag
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

		[InternalName("seasonOneTier")]
		public string SeasonOneTier
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

		[InternalName("nameChangeFlag")]
		public bool NameChangeFlag
		{
			get;
			set;
		}

		[InternalName("tutorialFlag")]
		public bool TutorialFlag
		{
			get;
			set;
		}

		[InternalName("socialNetworkUserIds")]
		public List<object> SocialNetworkUserIds
		{
			get;
			set;
		}

		public Summoner()
		{
		}

		public Summoner(Summoner.Callback callback)
		{
			this.callback = callback;
		}

		public Summoner(TypedObject result)
		{
			base.SetFields<Summoner>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<Summoner>(this, result);
			this.callback(this);
		}
	}
}
