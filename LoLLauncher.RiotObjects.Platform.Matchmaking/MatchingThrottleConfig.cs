using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
	public class MatchingThrottleConfig : RiotGamesObject
	{
		public delegate void Callback(MatchingThrottleConfig result);

		private string type = "com.riotgames.platform.matchmaking.MatchingThrottleConfig";

		private MatchingThrottleConfig.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("limit")]
		public double Limit
		{
			get;
			set;
		}

		[InternalName("matchingThrottleProperties")]
		public List<object> MatchingThrottleProperties
		{
			get;
			set;
		}

		[InternalName("cacheName")]
		public string CacheName
		{
			get;
			set;
		}

		public MatchingThrottleConfig()
		{
		}

		public MatchingThrottleConfig(MatchingThrottleConfig.Callback callback)
		{
			this.callback = callback;
		}

		public MatchingThrottleConfig(TypedObject result)
		{
			base.SetFields<MatchingThrottleConfig>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<MatchingThrottleConfig>(this, result);
			this.callback(this);
		}
	}
}
