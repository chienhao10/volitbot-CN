using System;

namespace LoLLauncher.RiotObjects.Platform.Statistics
{
	public class RawStatDTO : RiotGamesObject
	{
		public delegate void Callback(RawStatDTO result);

		private string type = "com.riotgames.platform.statistics.RawStatDTO";

		private RawStatDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("value")]
		public double Value
		{
			get;
			set;
		}

		[InternalName("statTypeName")]
		public string StatTypeName
		{
			get;
			set;
		}

		public RawStatDTO()
		{
		}

		public RawStatDTO(RawStatDTO.Callback callback)
		{
			this.callback = callback;
		}

		public RawStatDTO(TypedObject result)
		{
			base.SetFields<RawStatDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<RawStatDTO>(this, result);
			this.callback(this);
		}
	}
}
