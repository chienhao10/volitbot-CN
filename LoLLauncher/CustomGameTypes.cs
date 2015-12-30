using System;

namespace LoLLauncher
{
	public enum CustomGameTypes
	{
		[StringValue("unknown")]
		Unknown1,
		[StringValue("Blind Pick")]
		BlindPick,
		[StringValue("Draft")]
		Draft,
		[StringValue("No Ban Draft")]
		NoBanDraft,
		[StringValue("AllRandom")]
		AllRandom,
		[StringValue("Tournament Draft")]
		TournamentDraft,
		[StringValue("Blind Draft")]
		BlindDraft,
		[StringValue("unknown")]
		Unknown2,
		[StringValue("unknown")]
		Unknown3,
		[StringValue("Tutorial")]
		Tutorial,
		[StringValue("Battle Training")]
		BattleTraining,
		[StringValue("Bugged Blind Pick")]
		BuggedBlindPick,
		[StringValue("Blind Random")]
		BlindRandom,
		[StringValue("Blind Duplicate")]
		BlindDuplicate
	}
}
