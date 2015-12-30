using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class FellowPlayerInfo : RiotGamesObject
	{
		public delegate void Callback(FellowPlayerInfo result);

		private string type = "com.riotgames.platform.statistics.FellowPlayerInfo";

		private FellowPlayerInfo.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("championId")]
		public double ChampionId
		{
			get;
			set;
		}

		[InternalName("teamId")]
		public int TeamId
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

		public FellowPlayerInfo()
		{
		}

		public FellowPlayerInfo(FellowPlayerInfo.Callback callback)
		{
			this.callback = callback;
		}

		public FellowPlayerInfo(TypedObject result)
		{
			base.SetFields<FellowPlayerInfo>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<FellowPlayerInfo>(this, result);
			this.callback(this);
		}
	}
}
