using System;

namespace LoLLauncher.RiotObjects.Platform.Login
{
	public class AuthenticationCredentials : RiotGamesObject
	{
		public delegate void Callback(AuthenticationCredentials result);

		private string type = "com.riotgames.platform.login.AuthenticationCredentials";

		private AuthenticationCredentials.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("oldPassword")]
		public object OldPassword
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

		[InternalName("securityAnswer")]
		public object SecurityAnswer
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

		[InternalName("partnerCredentials")]
		public object PartnerCredentials
		{
			get;
			set;
		}

		[InternalName("domain")]
		public string Domain
		{
			get;
			set;
		}

		[InternalName("ipAddress")]
		public string IpAddress
		{
			get;
			set;
		}

		[InternalName("clientVersion")]
		public string ClientVersion
		{
			get;
			set;
		}

		[InternalName("locale")]
		public string Locale
		{
			get;
			set;
		}

		[InternalName("authToken")]
		public string AuthToken
		{
			get;
			set;
		}

		[InternalName("operatingSystem")]
		public string OperatingSystem
		{
			get;
			set;
		}

		public AuthenticationCredentials()
		{
		}

		public AuthenticationCredentials(AuthenticationCredentials.Callback callback)
		{
			this.callback = callback;
		}

		public AuthenticationCredentials(TypedObject result)
		{
			base.SetFields<AuthenticationCredentials>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<AuthenticationCredentials>(this, result);
			this.callback(this);
		}
	}
}
