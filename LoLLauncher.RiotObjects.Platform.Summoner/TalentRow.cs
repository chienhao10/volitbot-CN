using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner
{
	public class TalentRow : RiotGamesObject
	{
		public delegate void Callback(TalentRow result);

		private string type = "com.riotgames.platform.summoner.TalentRow";

		private TalentRow.Callback callback;

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

		[InternalName("talents")]
		public List<Talent> Talents
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

		[InternalName("pointsToActivate")]
		public int PointsToActivate
		{
			get;
			set;
		}

		[InternalName("tltRowId")]
		public int TltRowId
		{
			get;
			set;
		}

		public TalentRow()
		{
		}

		public TalentRow(TalentRow.Callback callback)
		{
			this.callback = callback;
		}

		public TalentRow(TypedObject result)
		{
			base.SetFields<TalentRow>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TalentRow>(this, result);
			this.callback(this);
		}
	}
}
