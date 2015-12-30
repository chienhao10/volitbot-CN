using LoLLauncher;
using System;
using System.Net;
using VoliBot.BaseRegions;

namespace VoliBot
{
	public abstract class BaseRegion
	{
		public abstract string RegionName
		{
			get;
		}

		public abstract bool Garena
		{
			get;
		}

		public abstract string InternalName
		{
			get;
		}

		public abstract string ChatName
		{
			get;
		}

		public abstract Uri NewsAddress
		{
			get;
		}

		public abstract string Locale
		{
			get;
		}

		public abstract RegioN PVPRegion
		{
			get;
		}

		public abstract IPAddress[] PingAddresses
		{
			get;
		}

		public abstract Uri SpectatorLink
		{
			get;
		}

		public abstract string SpectatorIpAddress
		{
			get;
			set;
		}

		public abstract string Location
		{
			get;
		}

		public static BaseRegion GetRegion(string requestedRegion)
		{
			requestedRegion = requestedRegion.ToUpper();
			string key;
			switch (key = requestedRegion)
			{
			case "BR":
				return new BR();
			case "EUW":
				return new EUW();
			case "EUNE":
				return new EUNE();
			case "KR":
				return new KR();
			case "LAN":
				return new LAN();
			case "LAS":
				return new LAS();
			case "NA":
				return new NA();
			case "OCE":
				return new OCE();
			case "RU":
				return new RU();
			case "TR":
				return new TR();
			}
			return null;
		}
	}
}
