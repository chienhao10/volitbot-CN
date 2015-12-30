using LoLLauncher;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects.Platform.Trade;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VoliBot;
using VoliBot.LoLLauncher.RiotObjects.Platform.Messaging;
using VoliBot.Utils;

namespace RitoBot
{
	internal class OldVoliBot
	{
		public Process exeProcess;

		public GameDTO currentGame = new GameDTO();

		public ChampionDTO[] availableChampsArray;

		public LoginDataPacket loginPacket = new LoginDataPacket();

		public LoLConnection connection = new LoLConnection();

		public List<ChampionDTO> availableChamps = new List<ChampionDTO>();

		public bool firstTimeInLobby = true;

		public bool firstTimeInQueuePop = true;

		public bool firstTimeInCustom = true;

		public bool firstTimeInPostChampSelect = true;

		public bool reAttempt;

		public string Accountname;

		public string Password;

		public string ipath;

		public string errorMSG1;

		public string errorMSG2;

		public BaseRegion baseRegion;

		public int relogTry;

		public console parent;

		public string sumName
		{
			get;
			set;
		}

		public double sumId
		{
			get;
			set;
		}

		public double sumLevel
		{
			get;
			set;
		}

		public double archiveSumLevel
		{
			get;
			set;
		}

		public double rpBalance
		{
			get;
			set;
		}

		public QueueTypes queueType
		{
			get;
			set;
		}

		public QueueTypes actualQueueType
		{
			get;
			set;
		}

		public int m_leaverBustedPenalty
		{
			get;
			set;
		}

		public string m_accessToken
		{
			get;
			set;
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		public OldVoliBot(string username, string password, console _parent, QueueTypes queue)
		{
			this.parent = _parent;
			this.ipath = this.parent.lolPath;
			this.Accountname = username;
			this.Password = password;
			this.queueType = queue;
			this.baseRegion = BaseRegion.GetRegion(_parent.region.ToString());
			this.connection.OnConnect += new LoLConnection.OnConnectHandler(this.connection_OnConnect);
			this.connection.OnDisconnect += new LoLConnection.OnDisconnectHandler(this.connection_OnDisconnect);
			this.connection.OnError += new LoLConnection.OnErrorHandler(this.connection_OnError);
			this.connection.OnLogin += new LoLConnection.OnLoginHandler(this.connection_OnLogin);
			this.connection.OnLoginQueueUpdate += new LoLConnection.OnLoginQueueUpdateHandler(this.connection_OnLoginQueueUpdate);
			this.connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(this.connection_OnMessageReceived);
			string password2 = Regex.Replace(password, "\\s+", "");
			this.connection.Connect(this.Accountname, password2, this.baseRegion.PVPRegion, _parent.currentVersion + "." + Config.clientSubVersion);
		}

		public async void connection_OnMessageReceived(object sender, object message)
		{
			if (message is GameDTO)
			{
				GameDTO gameDTO = message as GameDTO;
				string gameState;
				switch (gameState = gameDTO.GameState)
				{
				case "START_REQUESTED":
					return;
				case "FAILED_TO_START":
					this.parent.updateStatus(msgStatus.ERROR, "Failed to Start", this.Accountname);
					return;
				case "CHAMP_SELECT":
				{
					this.firstTimeInCustom = true;
					this.firstTimeInQueuePop = true;
					if (!this.firstTimeInLobby)
					{
						return;
					}
					this.firstTimeInLobby = false;
					this.updateStatus("In Champion Select", this.Accountname);
					await this.connection.SetClientReceivedGameMessage(gameDTO.Id, "CHAMP_SELECT_CLIENT");
					if (this.queueType == QueueTypes.ARAM)
					{
						return;
					}
					if (this.parent.championToPick != "" && this.parent.championToPick != "RANDOM")
					{
						int spellOneId;
						int spellTwoId;
						if (!this.parent.randomSpell)
						{
							spellOneId = Enums.spellToId(this.parent.spell1);
							spellTwoId = Enums.spellToId(this.parent.spell2);
						}
						else
						{
							Random random = new Random();
							List<int> list = new List<int>
							{
								13,
								6,
								7,
								10,
								1,
								11,
								21,
								12,
								3,
								14,
								2,
								4
							};
							int index = random.Next(list.Count);
							int index2 = random.Next(list.Count);
							int num2 = list[index];
							int num3 = list[index2];
							if (num2 == num3)
							{
								int index3 = random.Next(list.Count);
								num3 = list[index3];
							}
							spellOneId = Convert.ToInt32(num2);
							spellTwoId = Convert.ToInt32(num3);
						}
						await this.connection.SelectSpells(spellOneId, spellTwoId);
						await this.connection.SelectChampion(Enums.championToId(this.parent.championToPick));
						await this.connection.ChampionSelectCompleted();
						return;
					}
					if (this.parent.championToPick == "RANDOM")
					{
						int spellOneId2;
						int spellTwoId2;
						if (!this.parent.randomSpell)
						{
							spellOneId2 = Enums.spellToId(this.parent.spell1);
							spellTwoId2 = Enums.spellToId(this.parent.spell2);
						}
						else
						{
							Random random2 = new Random();
							List<int> list2 = new List<int>
							{
								13,
								6,
								7,
								10,
								1,
								11,
								21,
								12,
								3,
								14,
								2,
								4
							};
							int index4 = random2.Next(list2.Count);
							int index5 = random2.Next(list2.Count);
							int num4 = list2[index4];
							int num5 = list2[index5];
							if (num4 == num5)
							{
								int index6 = random2.Next(list2.Count);
								num5 = list2[index6];
							}
							spellOneId2 = Convert.ToInt32(num4);
							spellTwoId2 = Convert.ToInt32(num5);
						}
						await this.connection.SelectSpells(spellOneId2, spellTwoId2);
						IEnumerable<ChampionDTO> source = this.availableChampsArray.Shuffle<ChampionDTO>();
						await this.connection.SelectChampion(source.First((ChampionDTO champ) => champ.Owned || champ.FreeToPlay).ChampionId);
						await this.connection.ChampionSelectCompleted();
						return;
					}
					int spellOneId3;
					int spellTwoId3;
					if (!this.parent.randomSpell)
					{
						spellOneId3 = Enums.spellToId(this.parent.spell1);
						spellTwoId3 = Enums.spellToId(this.parent.spell2);
					}
					else
					{
						Random random3 = new Random();
						List<int> list3 = new List<int>
						{
							13,
							6,
							7,
							10,
							1,
							11,
							21,
							12,
							3,
							14,
							2,
							4
						};
						int index7 = random3.Next(list3.Count);
						int index8 = random3.Next(list3.Count);
						int num6 = list3[index7];
						int num7 = list3[index8];
						if (num6 == num7)
						{
							int index9 = random3.Next(list3.Count);
							num7 = list3[index9];
						}
						spellOneId3 = Convert.ToInt32(num6);
						spellTwoId3 = Convert.ToInt32(num7);
					}
					await this.connection.SelectSpells(spellOneId3, spellTwoId3);
					await this.connection.SelectChampion(this.availableChampsArray.First((ChampionDTO champ) => champ.Owned || champ.FreeToPlay).ChampionId);
					await this.connection.ChampionSelectCompleted();
					return;
				}
				case "POST_CHAMP_SELECT":
					this.firstTimeInLobby = false;
					if (this.firstTimeInPostChampSelect)
					{
						this.firstTimeInPostChampSelect = false;
						this.updateStatus("(Post Champ Select)", this.Accountname);
						return;
					}
					return;
				case "IN_QUEUE":
					this.updateStatus("In Queue", this.Accountname);
					return;
				case "TERMINATED":
					this.updateStatus("Re-entering queue", this.Accountname);
					this.firstTimeInPostChampSelect = true;
					this.firstTimeInQueuePop = true;
					return;
				case "JOINING_CHAMP_SELECT":
					if (this.firstTimeInQueuePop && gameDTO.StatusOfParticipants.Contains("1"))
					{
						this.updateStatus("Accepted Queue", this.Accountname);
						this.firstTimeInQueuePop = false;
						this.firstTimeInLobby = true;
						await this.connection.AcceptPoppedGame(true);
						return;
					}
					return;
				}
				this.updateStatus("[DEFAULT]" + gameDTO.GameStateString, this.Accountname);
			}
			else if (message.GetType() == typeof(TradeContractDTO))
			{
				TradeContractDTO tradeContractDTO = message as TradeContractDTO;
				if (tradeContractDTO != null)
				{
					string expr_CDE = tradeContractDTO.State;
					if (expr_CDE != null && expr_CDE == "PENDING" && tradeContractDTO != null)
					{
						await this.connection.AcceptTrade(tradeContractDTO.RequesterInternalSummonerName, (int)tradeContractDTO.RequesterChampionId);
					}
				}
			}
			else if (message is PlayerCredentialsDto)
			{
				this.firstTimeInPostChampSelect = true;
				PlayerCredentialsDto playerCredentialsDto = message as PlayerCredentialsDto;
				ProcessStartInfo startInfo = new ProcessStartInfo();
				startInfo.CreateNoWindow = false;
				startInfo.WorkingDirectory = this.FindLoLExe();
				startInfo.FileName = "League of Legends.exe";
				startInfo.Arguments = string.Concat(new object[]
				{
					"\"8394\" \"LoLLauncher.exe\" \"\" \"",
					playerCredentialsDto.ServerIp,
					" ",
					playerCredentialsDto.ServerPort,
					" ",
					playerCredentialsDto.EncryptionKey,
					" ",
					playerCredentialsDto.SummonerId,
					"\""
				});
				this.updateStatus("Launching League of Legends\n", this.Accountname);
				new Thread(delegate
				{
					this.exeProcess = Process.Start(startInfo);
					this.exeProcess.Exited += new EventHandler(this.exeProcess_Exited);
					while (this.exeProcess.MainWindowHandle == IntPtr.Zero)
					{
					}
					this.exeProcess.PriorityClass = ProcessPriorityClass.Idle;
					this.exeProcess.EnableRaisingEvents = true;
				}).Start();
			}
			else if (message is EndOfGameStats)
			{
				if (this.exeProcess != null)
				{
					this.exeProcess.Exited -= new EventHandler(this.exeProcess_Exited);
					this.exeProcess.Kill();
					Thread.Sleep(500);
					if (this.exeProcess.Responding)
					{
						Process.Start("taskkill /F /IM \"League of Legends.exe\"");
					}
					this.loginPacket = await this.connection.GetLoginDataPacketForUser();
					this.archiveSumLevel = this.sumLevel;
					this.sumLevel = this.loginPacket.AllSummonerData.SummonerLevel.Level;
					if (this.sumLevel != this.archiveSumLevel)
					{
						this.levelUp();
					}
				}
				this.AttachToQueue();
			}
		}

		private async void AttachToQueue()
		{
			MatchMakerParams matchMakerParams = new MatchMakerParams();
			if (this.queueType == QueueTypes.INTRO_BOT)
			{
				matchMakerParams.BotDifficulty = "INTRO";
			}
			else if (this.queueType == QueueTypes.BEGINNER_BOT)
			{
				matchMakerParams.BotDifficulty = "EASY";
			}
			else if (this.queueType == QueueTypes.MEDIUM_BOT)
			{
				matchMakerParams.BotDifficulty = "MEDIUM";
			}
			if (this.sumLevel == 3.0 && this.actualQueueType == QueueTypes.NORMAL_5x5)
			{
				this.queueType = this.actualQueueType;
			}
			else if (this.sumLevel == 6.0 && this.actualQueueType == QueueTypes.ARAM)
			{
				this.queueType = this.actualQueueType;
			}
			else if (this.sumLevel == 7.0 && this.actualQueueType == QueueTypes.NORMAL_3x3)
			{
				this.queueType = this.actualQueueType;
			}
			matchMakerParams.QueueIds = new int[]
			{
				(int)this.queueType
			};
			SearchingForMatchNotification searchingForMatchNotification = await this.connection.AttachToQueue(matchMakerParams);
			if (searchingForMatchNotification.PlayerJoinFailures == null)
			{
				this.updateStatus("In Queue: " + this.queueType.ToString(), this.Accountname);
			}
			else
			{
				foreach (QueueDodger current in searchingForMatchNotification.PlayerJoinFailures)
				{
					if (current.ReasonFailed == "LEAVER_BUSTED")
					{
						this.m_accessToken = current.AccessToken;
						if (current.LeaverPenaltyMillisRemaining > this.m_leaverBustedPenalty)
						{
							this.m_leaverBustedPenalty = current.LeaverPenaltyMillisRemaining;
						}
					}
					else if (current.ReasonFailed == "LEAVER_BUSTER_TAINTED_WARNING")
					{
						await this.connection.ackLeaverBusterWarning();
						await this.connection.callPersistenceMessaging(new SimpleDialogMessageResponse
						{
							AccountID = this.loginPacket.AllSummonerData.Summoner.SumId,
							MsgID = this.loginPacket.AllSummonerData.Summoner.SumId,
							Command = "ack"
						});
						this.connection_OnMessageReceived(null, new EndOfGameStats());
					}
				}
				if (!string.IsNullOrEmpty(this.m_accessToken))
				{
					this.updateStatus("Waiting out leaver buster: " + (float)(this.m_leaverBustedPenalty / 1000) / 60f + " minutes!", this.Accountname);
					Thread.Sleep(TimeSpan.FromMilliseconds((double)this.m_leaverBustedPenalty));
					searchingForMatchNotification = await this.connection.AttachToLowPriorityQueue(matchMakerParams, this.m_accessToken);
					if (searchingForMatchNotification.PlayerJoinFailures == null)
					{
						this.updateStatus("Succesfully joined lower priority queue!", this.Accountname);
					}
					else
					{
						this.updateStatus("There was an error in joining lower priority queue.\nDisconnecting.", this.Accountname);
						this.connection.Disconnect();
						this.parent.lognNewAccount();
					}
				}
			}
		}

		private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
		{
			if (positionInLine <= 0)
			{
				return;
			}
			this.updateStatus("Position to login: " + positionInLine, this.Accountname);
		}

		private void connection_OnLogin(object sender, string username, string ipAddress)
		{
			OldVoliBot.<>c__DisplayClass21 <>c__DisplayClass = new OldVoliBot.<>c__DisplayClass21();
			<>c__DisplayClass.sender = sender;
			<>c__DisplayClass.<>4__this = this;
			new Thread(delegate
			{
				OldVoliBot.<>c__DisplayClass21.<<connection_OnLogin>b__20>d__23 <<connection_OnLogin>b__20>d__;
				<<connection_OnLogin>b__20>d__.<>4__this = <>c__DisplayClass;
				<<connection_OnLogin>b__20>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<connection_OnLogin>b__20>d__.<>1__state = -1;
				AsyncVoidMethodBuilder <>t__builder = <<connection_OnLogin>b__20>d__.<>t__builder;
				<>t__builder.Start<OldVoliBot.<>c__DisplayClass21.<<connection_OnLogin>b__20>d__23>(ref <<connection_OnLogin>b__20>d__);
			}).Start();
		}

		private void connection_OnError(object sender, Error error)
		{
			if (error.Type == ErrorType.AuthKey || error.Type == ErrorType.General)
			{
				if (this.reAttempt)
				{
					return;
				}
				this.updateStatus("无法连接，重新连接", this.Accountname);
				this.reAttempt = true;
				this.connection.Connect(this.Accountname, this.Password, this.baseRegion.PVPRegion, Config.clientSeason + "." + Config.clientSubVersion);
				return;
			}
			else
			{
				if (error.Message.Contains("此召唤师为拥有"))
				{
					return;
				}
				if (error.Message.Contains("召唤师等级不够无法选择技能"))
				{
					Random random = new Random();
					List<int> list = new List<int>
					{
						13,
						6,
						7,
						10,
						1,
						11,
						21,
						12,
						3,
						14,
						2,
						4
					};
					int index = random.Next(list.Count);
					int index2 = random.Next(list.Count);
					int num = list[index];
					int num2 = list[index2];
					if (num == num2)
					{
						int index3 = random.Next(list.Count);
						num2 = list[index3];
					}
					Convert.ToInt32(num);
					Convert.ToInt32(num2);
					return;
				}
				if (error.Message.Contains("无法获得验证码"))
				{
					this.updateStatus("登陆失败", this.Accountname);
					return;
				}
				this.updateStatus(string.Concat(new object[]
				{
					"[",
					error.Type,
					"]error received:\n",
					error.Message
				}), this.Accountname);
				return;
			}
		}

		private void connection_OnDisconnect(object sender, EventArgs e)
		{
			this.updateStatus("Disconnected", this.Accountname);
		}

		private void connection_OnConnect(object sender, EventArgs e)
		{
		}

		private async void exeProcess_Exited(object sender, EventArgs e)
		{
			this.updateStatus("Restart League of Legends.", this.Accountname);
			this.loginPacket = await this.connection.GetLoginDataPacketForUser();
			if (this.loginPacket.ReconnectInfo != null && this.loginPacket.ReconnectInfo.Game != null)
			{
				this.connection_OnMessageReceived(sender, this.loginPacket.ReconnectInfo.PlayerCredentials);
			}
			else
			{
				this.connection_OnMessageReceived(sender, new EndOfGameStats());
			}
		}

		private void updateStatus(string status, string accname)
		{
			this.parent.updateStatus(msgStatus.INFO, status, accname);
		}

		private void levelUp()
		{
			this.updateStatus("Level Up: " + this.sumLevel, this.Accountname);
			this.rpBalance = this.loginPacket.RpBalance;
			if (this.sumLevel >= (double)this.parent.maxLevel)
			{
				this.connection.Disconnect();
				if (!this.connection.IsConnected())
				{
					this.parent.lognNewAccount();
				}
			}
			if (this.rpBalance == 400.0 && this.parent.buyBoost && this.sumLevel < 5.0)
			{
				this.updateStatus("购买双倍经验卡", this.Accountname);
				try
				{
					Task task = new Task(new Action(this.buyBoost));
					task.Start();
				}
				catch (Exception arg)
				{
					this.updateStatus("无法购买双倍经验卡" + arg, this.Accountname);
				}
			}
		}

		private async void buyBoost()
		{
			try
			{
				string requestUri = await this.connection.GetStoreUrl();
				HttpClient httpClient = new HttpClient();
				await httpClient.GetStringAsync(requestUri);
				string requestUri2 = "https://store." + this.baseRegion.ChatName + ".lol.riotgames.com/store/tabs/view/boosts/1";
				await httpClient.GetStringAsync(requestUri2);
				string requestUri3 = "https://store." + this.baseRegion.ChatName + ".lol.riotgames.com/store/purchase/item";
				HttpContent content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("item_id", "boosts_2"),
					new KeyValuePair<string, string>("currency_type", "rp"),
					new KeyValuePair<string, string>("quantity", "1"),
					new KeyValuePair<string, string>("rp", "260"),
					new KeyValuePair<string, string>("ip", "null"),
					new KeyValuePair<string, string>("duration_type", "PURCHASED"),
					new KeyValuePair<string, string>("duration", "3")
				});
				await httpClient.PostAsync(requestUri3, content);
				this.updateStatus("Bought 'XP Boost: 3 Days'!", this.Accountname);
				httpClient.Dispose();
			}
			catch (Exception ex)
			{
				this.parent.updateStatus(msgStatus.ERROR, ex.Message, this.Accountname);
			}
		}

		private string FindLoLExe()
		{
			string text = this.ipath;
			if (text.Contains("未找到"))
			{
				return text;
			}
			text += "RADS\\solutions\\lol_game_client_sln\\releases\\";
			text = (from f in Directory.EnumerateDirectories(text)
			orderby new DirectoryInfo(f).CreationTime
			select f).Last<string>();
			return text + "\\deploy\\";
		}
	}
}
