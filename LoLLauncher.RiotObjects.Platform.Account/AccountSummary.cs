using System;

namespace LoLLauncher.RiotObjects.Platform.Account
{
	public class AccountSummary : RiotGamesObject
	{
		public delegate void Callback(AccountSummary result);

		private string type = "com.riotgames.platform.account.AccountSummary";

		private AccountSummary.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("groupCount")]
		public int GroupCount
		{
			get;
			set;
		}

		[InternalName("username")]
		public string Username
		{
			get;
			set;
		}

		[InternalName("accountId")]
		public double AccountId
		{
			get;
			set;
		}

		[InternalName("summonerInternalName")]
		public object SummonerInternalName
		{
			get;
			set;
		}

		[InternalName("admin")]
		public bool Admin
		{
			get;
			set;
		}

		[InternalName("hasBetaAccess")]
		public bool HasBetaAccess
		{
			get;
			set;
		}

		[InternalName("summonerName")]
		public object SummonerName
		{
			get;
			set;
		}

		[InternalName("partnerMode")]
		public bool PartnerMode
		{
			get;
			set;
		}

		[InternalName("needsPasswordReset")]
		public bool NeedsPasswordReset
		{
			get;
			set;
		}

		public AccountSummary()
		{
		}

		public AccountSummary(AccountSummary.Callback callback)
		{
			this.callback = callback;
		}

		public AccountSummary(TypedObject result)
		{
			base.SetFields<AccountSummary>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AccountSummary>(this, result);
			this.callback(this);
		}
	}
}
