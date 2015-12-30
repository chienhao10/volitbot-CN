using System;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Masterybook
{
	public class TalentEntry : RiotGamesObject
	{
		public delegate void Callback(TalentEntry result);

		private string type = "com.riotgames.platform.summoner.masterybook.TalentEntry";

		private TalentEntry.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("rank")]
		public int Rank
		{
			get;
			set;
		}

		[InternalName("talentId")]
		public int TalentId
		{
			get;
			set;
		}

		[InternalName("talent")]
		public Talent Talent
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

		public TalentEntry()
		{
		}

		public TalentEntry(TalentEntry.Callback callback)
		{
			this.callback = callback;
		}

		public TalentEntry(TypedObject result)
		{
			base.SetFields<TalentEntry>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TalentEntry>(this, result);
			this.callback(this);
		}
	}
}
