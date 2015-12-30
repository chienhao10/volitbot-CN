using System;

namespace LoLLauncher.RiotObjects.Leagues.Pojo
{
	public class MiniSeriesDTO : RiotGamesObject
	{
		public delegate void Callback(MiniSeriesDTO result);

		private string type = "com.riotgames.leagues.pojo.MiniSeriesDTO";

		private MiniSeriesDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("progress")]
		public string Progress
		{
			get;
			set;
		}

		[InternalName("target")]
		public int Target
		{
			get;
			set;
		}

		[InternalName("losses")]
		public int Losses
		{
			get;
			set;
		}

		[InternalName("timeLeftToPlayMillis")]
		public double TimeLeftToPlayMillis
		{
			get;
			set;
		}

		[InternalName("wins")]
		public int Wins
		{
			get;
			set;
		}

		public MiniSeriesDTO()
		{
		}

		public MiniSeriesDTO(MiniSeriesDTO.Callback callback)
		{
			this.callback = callback;
		}

		public MiniSeriesDTO(TypedObject result)
		{
			base.SetFields<MiniSeriesDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<MiniSeriesDTO>(this, result);
			this.callback(this);
		}
	}
}
