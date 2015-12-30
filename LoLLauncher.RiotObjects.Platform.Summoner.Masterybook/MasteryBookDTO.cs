using System;
using System.Collections.Generic;

namespace LoLLauncher.RiotObjects.Platform.Summoner.Masterybook
{
	public class MasteryBookDTO : RiotGamesObject
	{
		public delegate void Callback(MasteryBookDTO result);

		private string type = "com.riotgames.platform.summoner.masterybook.MasteryBookDTO";

		private MasteryBookDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("bookPagesJson")]
		public object BookPagesJson
		{
			get;
			set;
		}

		[InternalName("bookPages")]
		public List<MasteryBookPageDTO> BookPages
		{
			get;
			set;
		}

		[InternalName("dateString")]
		public string DateString
		{
			get;
			set;
		}

		[InternalName("summonerId")]
		public double SummonerId
		{
			get;
			set;
		}

		public MasteryBookDTO()
		{
		}

		public MasteryBookDTO(MasteryBookDTO.Callback callback)
		{
			this.callback = callback;
		}

		public MasteryBookDTO(TypedObject result)
		{
			base.SetFields<MasteryBookDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<MasteryBookDTO>(this, result);
			this.callback(this);
		}
	}
}
