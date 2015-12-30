using System;

namespace LoLLauncher
{
	public enum GameMode
	{
		[StringValue("CLASSIC")]
		SummonersRift = 1,
		[StringValue("ODIN")]
		Dominion = 8,
		[StringValue("CLASSIC")]
		TwistedTreeline = 10,
		[StringValue("ARAM")]
		HowlingAbyss = 12,
		[StringValue("TUTORIAL")]
		Tutorial
	}
}
