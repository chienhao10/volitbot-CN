using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Masterybook
{
	public class MasteryBookPageDTO : RiotGamesObject
	{
		public delegate void Callback(MasteryBookPageDTO result);

		private string type = "com.riotgames.platform.summoner.masterybook.MasteryBookPageDTO";

		private MasteryBookPageDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("talentEntries")]
		public List<TalentEntry> TalentEntries
		{
			get;
			set;
		}

		[InternalName("pageId")]
		public double PageId
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

		[InternalName("current")]
		public bool Current
		{
			get;
			set;
		}

		[InternalName("createDate")]
		public object CreateDate
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

		public MasteryBookPageDTO()
		{
		}

		public MasteryBookPageDTO(MasteryBookPageDTO.Callback callback)
		{
			this.callback = callback;
		}

		public MasteryBookPageDTO(TypedObject result)
		{
			base.SetFields<MasteryBookPageDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<MasteryBookPageDTO>(this, result);
			this.callback(this);
		}
	}
}
