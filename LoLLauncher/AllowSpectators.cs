using System;

namespace LoLLauncher
{
	public enum AllowSpectators
	{
		[StringValue("ALL")]
		All = 1,
		[StringValue("LOBBYONLY")]
		LobbyOnly,
		[StringValue("DROPINONLY")]
		DropInOnly,
		[StringValue("NONE")]
		None = 0
	}
}
