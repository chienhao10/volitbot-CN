using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class TalentGroup : RiotGamesObject
	{
		public delegate void Callback(TalentGroup result);

		private string type = "com.riotgames.platform.summoner.TalentGroup";

		private TalentGroup.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("index")]
		public int Index
		{
			get;
			set;
		}

		[InternalName("talentRows")]
		public List<TalentRow> TalentRows
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

		[InternalName("tltGroupId")]
		public int TltGroupId
		{
			get;
			set;
		}

		public TalentGroup()
		{
		}

		public TalentGroup(TalentGroup.Callback callback)
		{
			this.callback = callback;
		}

		public TalentGroup(TypedObject result)
		{
			base.SetFields<TalentGroup>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TalentGroup>(this, result);
			this.callback(this);
		}
	}
}
