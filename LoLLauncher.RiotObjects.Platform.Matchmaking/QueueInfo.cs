using System;

namespace LoLLauncher.RiotObjects.Platform.Matchmaking
{
	public class QueueInfo : RiotGamesObject
	{
		public delegate void Callback(QueueInfo result);

		private string type = "com.riotgames.platform.matchmaking.QueueInfo";

		private QueueInfo.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("waitTime")]
		public double WaitTime
		{
			get;
			set;
		}

		[InternalName("queueId")]
		public double QueueId
		{
			get;
			set;
		}

		[InternalName("queueLength")]
		public int QueueLength
		{
			get;
			set;
		}

		public QueueInfo()
		{
		}

		public QueueInfo(QueueInfo.Callback callback)
		{
			this.callback = callback;
		}

		public QueueInfo(TypedObject result)
		{
			base.SetFields<QueueInfo>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<QueueInfo>(this, result);
			this.callback(this);
		}
	}
}
