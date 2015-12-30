using System;

namespace LoLLauncher.RiotObjects.Platform.Reroll.Pojo
{
	internal class EogPointChangeBreakdown : RiotGamesObject
	{
		public delegate void Callback(EogPointChangeBreakdown result);

		private string type = "com.riotgames.platform.reroll.pojo.EogPointChangeBreakdown";

		private EogPointChangeBreakdown.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("pointChangeFromGamePlay")]
		public double PointChangeFromGamePlay
		{
			get;
			set;
		}

		[InternalName("pointChangeFromChampionsOwned")]
		public double PointChangeFromChampionsOwned
		{
			get;
			set;
		}

		[InternalName("previousPoints")]
		public double PreviousPoints
		{
			get;
			set;
		}

		[InternalName("pointsUsed")]
		public double PointsUsed
		{
			get;
			set;
		}

		[InternalName("endPoints")]
		public double EndPoints
		{
			get;
			set;
		}

		public EogPointChangeBreakdown()
		{
		}

		public EogPointChangeBreakdown(EogPointChangeBreakdown.Callback callback)
		{
			this.callback = callback;
		}

		public EogPointChangeBreakdown(TypedObject result)
		{
			base.SetFields<EogPointChangeBreakdown>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<EogPointChangeBreakdown>(this, result);
			this.callback(this);
		}
	}
}
