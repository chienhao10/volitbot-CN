using System;

namespace LoLLauncher
{
	public enum GameType
	{
		[StringValue("RANKED_TEAM_GAME")]
		RankedTeamGame,
		[StringValue("RANKED_GAME")]
		RankedGame,
		[StringValue("NORMAL_GAME")]
		NormalGame,
		[StringValue("CUSTOM_GAME")]
		CustomGame,
		[StringValue("TUTORIAL_GAME")]
		TutorialGame,
		[StringValue("PRACTICE_GAME")]
		PracticeGame,
		[StringValue("RANKED_GAME_SOLO")]
		RankedGameSolo,
		[StringValue("COOP_VS_AI")]
		CoopVsAi,
		[StringValue("RANKED_GAME_PREMADE")]
		RankedGamePremade
	}
}
