using System;

namespace LoLLauncher.RiotObjects.Kudos.Dto
{
	public class PendingKudosDTO : RiotGamesObject
	{
		public delegate void Callback(PendingKudosDTO result);

		private string type = "com.riotgames.kudos.dto.PendingKudosDTO";

		private PendingKudosDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("pendingCounts")]
		public int[] PendingCounts
		{
			get;
			set;
		}

		public PendingKudosDTO()
		{
		}

		public PendingKudosDTO(PendingKudosDTO.Callback callback)
		{
			this.callback = callback;
		}

		public PendingKudosDTO(TypedObject result)
		{
			base.SetFields<PendingKudosDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PendingKudosDTO>(this, result);
			this.callback(this);
		}
	}
}
