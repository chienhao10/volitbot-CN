using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class TimeTrackedStat : RiotGamesObject
	{
		public delegate void Callback(TimeTrackedStat result);

		private string type = "com.riotgames.platform.statistics.TimeTrackedStat";

		private TimeTrackedStat.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("timestamp")]
		public DateTime Timestamp
		{
			get;
			set;
		}

		[InternalName("type")]
		public string Type
		{
			get;
			set;
		}

		public TimeTrackedStat()
		{
		}

		public TimeTrackedStat(TimeTrackedStat.Callback callback)
		{
			this.callback = callback;
		}

		public TimeTrackedStat(TypedObject result)
		{
			base.SetFields<TimeTrackedStat>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TimeTrackedStat>(this, result);
			this.callback(this);
		}
	}
}
