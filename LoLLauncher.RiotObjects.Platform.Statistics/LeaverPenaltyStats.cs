using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class LeaverPenaltyStats : RiotGamesObject
	{
		public delegate void Callback(LeaverPenaltyStats result);

		private string type = "com.riotgames.platform.statistics.LeaverPenaltyStats";

		private LeaverPenaltyStats.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("lastLevelIncrease")]
		public object LastLevelIncrease
		{
			get;
			set;
		}

		[InternalName("level")]
		public int Level
		{
			get;
			set;
		}

		[InternalName("lastDecay")]
		public DateTime LastDecay
		{
			get;
			set;
		}

		[InternalName("userInformed")]
		public bool UserInformed
		{
			get;
			set;
		}

		[InternalName("points")]
		public int Points
		{
			get;
			set;
		}

		public LeaverPenaltyStats()
		{
		}

		public LeaverPenaltyStats(LeaverPenaltyStats.Callback callback)
		{
			this.callback = callback;
		}

		public LeaverPenaltyStats(TypedObject result)
		{
			base.SetFields<LeaverPenaltyStats>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<LeaverPenaltyStats>(this, result);
			this.callback(this);
		}
	}
}
