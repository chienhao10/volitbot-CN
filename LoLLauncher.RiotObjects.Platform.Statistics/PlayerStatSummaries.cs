using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class PlayerStatSummaries : RiotGamesObject
	{
		public delegate void Callback(PlayerStatSummaries result);

		private string type = "com.riotgames.platform.statistics.PlayerStatSummaries";

		private PlayerStatSummaries.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("playerStatSummarySet")]
		public List<PlayerStatSummary> PlayerStatSummarySet
		{
			get;
			set;
		}

		[InternalName("userId")]
		public double UserId
		{
			get;
			set;
		}

		public PlayerStatSummaries()
		{
		}

		public PlayerStatSummaries(PlayerStatSummaries.Callback callback)
		{
			this.callback = callback;
		}

		public PlayerStatSummaries(TypedObject result)
		{
			base.SetFields<PlayerStatSummaries>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlayerStatSummaries>(this, result);
			this.callback(this);
		}
	}
}
