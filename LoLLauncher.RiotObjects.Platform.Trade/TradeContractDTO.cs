using System;

namespace LoLLauncher.RiotObjects.Platform.Trade
{
	public class TradeContractDTO : RiotGamesObject
	{
		public delegate void Callback(TradeContractDTO result);

		private string type = "com.riotgames.platform.trade.api.contract.TradeContractDTO";

		private TradeContractDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("requesterInternalSummonerName")]
		public string RequesterInternalSummonerName
		{
			get;
			set;
		}

		[InternalName("requesterChampionId")]
		public double RequesterChampionId
		{
			get;
			set;
		}

		[InternalName("state")]
		public string State
		{
			get;
			set;
		}

		[InternalName("responderChampionId")]
		public double ResponderChampionId
		{
			get;
			set;
		}

		[InternalName("responderInternalSummonerName")]
		public string ResponderInternalSummonerName
		{
			get;
			set;
		}

		public TradeContractDTO()
		{
		}

		public TradeContractDTO(TradeContractDTO.Callback callback)
		{
			this.callback = callback;
		}

		public TradeContractDTO(TypedObject result)
		{
			base.SetFields<TradeContractDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<TradeContractDTO>(this, result);
			this.callback(this);
		}
	}
}
