using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class RawStat : RiotGamesObject
	{
		public delegate void Callback(RawStat result);

		private string type = "com.riotgames.platform.statistics.RawStat";

		private RawStat.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("statType")]
		public string StatType
		{
			get;
			set;
		}

		[InternalName("value")]
		public double Value
		{
			get;
			set;
		}

		public RawStat()
		{
		}

		public RawStat(RawStat.Callback callback)
		{
			this.callback = callback;
		}

		public RawStat(TypedObject result)
		{
			base.SetFields<RawStat>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<RawStat>(this, result);
			this.callback(this);
		}
	}
}
