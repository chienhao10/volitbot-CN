using LoLLauncher.RiotObjects.Platform.Account;
using System;

namespace LoLLauncher.RiotObjects.Platform.Login
{
	public class Session : RiotGamesObject
	{
		public delegate void Callback(Session result);

		private string type = "com.riotgames.platform.login.Session";

		private Session.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("token")]
		public string Token
		{
			get;
			set;
		}

		[InternalName("password")]
		public string Password
		{
			get;
			set;
		}

		[InternalName("accountSummary")]
		public AccountSummary AccountSummary
		{
			get;
			set;
		}

		public Session()
		{
		}

		public Session(Session.Callback callback)
		{
			this.callback = callback;
		}

		public Session(TypedObject result)
		{
			base.SetFields<Session>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<Session>(this, result);
			this.callback(this);
		}
	}
}
