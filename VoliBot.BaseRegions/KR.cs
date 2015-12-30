using LoLLauncher;
using System;
using System.Net;

namespace VoliBot.BaseRegions
{
	public class KR : BaseRegion
	{
		public override string Location
		{
			get
			{
				return null;
			}
		}

		public override string RegionName
		{
			get
			{
				return "KR";
			}
		}

		public override bool Garena
		{
			get
			{
				return false;
			}
		}

		public override string InternalName
		{
			get
			{
				return "KR";
			}
		}

		public override string Locale
		{
			get
			{
				return "ko_KR";
			}
		}

		public override string ChatName
		{
			get
			{
				return "kr";
			}
		}

		public override Uri NewsAddress
		{
			get
			{
				return new Uri("");
			}
		}

		public override RegioN PVPRegion
		{
			get
			{
				return RegioN.KR;
			}
		}

		public override IPAddress[] PingAddresses
		{
			get
			{
				return new IPAddress[0];
			}
		}

		public override Uri SpectatorLink
		{
			get
			{
				return new Uri("http://QFKR1PROXY.kassad.in:8088/observer-mode/rest/");
			}
		}

		public override string SpectatorIpAddress
		{
			get
			{
				return "110.45.191.11:80";
			}
			set
			{
			}
		}

		public void NASqlite()
		{
		}
	}
}
