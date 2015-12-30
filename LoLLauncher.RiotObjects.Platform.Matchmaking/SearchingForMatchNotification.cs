using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
	public class SearchingForMatchNotification : RiotGamesObject
	{
		public delegate void Callback(SearchingForMatchNotification result);

		private string type = "com.riotgames.platform.matchmaking.SearchingForMatchNotification";

		private SearchingForMatchNotification.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("playerJoinFailures")]
		public List<QueueDodger> PlayerJoinFailures
		{
			get;
			set;
		}

		[InternalName("ghostGameSummoners")]
		public object GhostGameSummoners
		{
			get;
			set;
		}

		[InternalName("joinedQueues")]
		public List<QueueInfo> JoinedQueues
		{
			get;
			set;
		}

		public SearchingForMatchNotification()
		{
		}

		public SearchingForMatchNotification(SearchingForMatchNotification.Callback callback)
		{
			this.callback = callback;
		}

		public SearchingForMatchNotification(TypedObject result)
		{
			base.SetFields<SearchingForMatchNotification>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<SearchingForMatchNotification>(this, result);
			this.callback(this);
		}
	}
}
