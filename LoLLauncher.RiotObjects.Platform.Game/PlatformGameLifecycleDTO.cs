using System;

namespace LoLLauncher.RiotObjects.Platform.Game
{
	public class PlatformGameLifecycleDTO : RiotGamesObject
	{
		public delegate void Callback(PlatformGameLifecycleDTO result);

		private string type = "com.riotgames.platform.game.PlatformGameLifecycleDTO";

		private PlatformGameLifecycleDTO.Callback callback;

		public override string TypeName
		{
			get
			{
				return this.type;
			}
		}

		[InternalName("gameSpecificLoyaltyRewards")]
		public object GameSpecificLoyaltyRewards
		{
			get;
			set;
		}

		[InternalName("reconnectDelay")]
		public int ReconnectDelay
		{
			get;
			set;
		}

		[InternalName("lastModifiedDate")]
		public object LastModifiedDate
		{
			get;
			set;
		}

		[InternalName("game")]
		public GameDTO Game
		{
			get;
			set;
		}

		[InternalName("playerCredentials")]
		public PlayerCredentialsDto PlayerCredentials
		{
			get;
			set;
		}

		[InternalName("gameName")]
		public string GameName
		{
			get;
			set;
		}

		[InternalName("connectivityStateEnum")]
		public object ConnectivityStateEnum
		{
			get;
			set;
		}

		public PlatformGameLifecycleDTO()
		{
		}

		public PlatformGameLifecycleDTO(PlatformGameLifecycleDTO.Callback callback)
		{
			this.callback = callback;
		}

		public PlatformGameLifecycleDTO(TypedObject result)
		{
			base.SetFields<PlatformGameLifecycleDTO>(this, result);
		}

		public override void DoCallback(TypedObject result)
		{
			base.SetFields<PlatformGameLifecycleDTO>(this, result);
			this.callback(this);
		}
	}
}
