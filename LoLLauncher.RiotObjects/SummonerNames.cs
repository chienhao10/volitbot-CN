using System;

namespace LoLLauncher.RiotObjects
{
	public class SummonerNames : RiotGamesObject
	{
		public delegate void Callback(object[] result);

		private SummonerNames.Callback callback;

		public SummonerNames(SummonerNames.Callback callback)
		{
			this.callback = callback;
		}

		public override void DoCallback(TypedObject result)
		{
			this.callback(result.GetArray("array"));
		}
	}
}
