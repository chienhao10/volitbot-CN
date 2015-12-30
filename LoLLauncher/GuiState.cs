using System;

namespace LoLLauncher
{
	[Flags]
	public enum GuiState
	{
		None = 1,
		LoggedOut = 2,
		LoggingIn = 4,
		LoggedIn = 8,
		CustomSearchGame = 16,
		CustomCreateGame = 32,
		GameLobby = 64
	}
}
