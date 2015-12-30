using System;

namespace LoLLauncher.RiotObjects.Platform.Reroll.Pojo
{
	public class PointSummary : RiotGamesObject
	{
		public delegate void Callback(PointSummary result);

		private string type = "com.riotgames.platform.reroll.pojo.PointSummary";

		private PointSummary.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("pointsToNextRoll")]
		public double PointsToNextRoll
		{
			get;
			set;
		}

		[InternalName("maxRolls")]
		public int MaxRolls
		{
			get;
			set;
		}

		[InternalName("numberOfRolls")]
		public int NumberOfRolls
		{
			get;
			set;
		}

		[InternalName("pointsCostToRoll")]
		public double PointsCostToRoll
		{
			get;
			set;
		}

		[InternalName("currentPoints")]
		public double CurrentPoints
		{
			get;
			set;
		}

		public PointSummary()
		{
		}

		public PointSummary(PointSummary.Callback callback)
		{
			this.callback = callback;
		}

		public PointSummary(TypedObject result)
		{
			base.SetFields<PointSummary>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PointSummary>(this, result);
			this.callback(this);
		}
	}
}
