using System;

namespace VoliBot
{
	public class AccountInBox
	{
		public string _username;

		public string _password;

		public string _region;

		public AccountInBox(string username, string password, string region)
		{
			this._username = username;
			this._password = password;
			this._region = region;
		}
	}
}
