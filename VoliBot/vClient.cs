using Ini;
using LoLLauncher;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Game.Message;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects.Platform.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using VoliBot.LoLLauncher.RiotObjects.Platform.Messaging;
using VoliBot.Properties;
using VoliBot.Utils;
using VoliBots;

namespace VoliBot
{
	public class vClient : Form
	{
		private const int GWL_STYLE = -16;

		private const long WS_VISIBLE = 268435456L;

		private const long WS_MAXIMIZE = 16777216L;

		private const long WS_BORDER = 8388608L;

		private const long WS_CHILD = 1073741824L;

		public VoliBot _parent;

		public string _username = "";

		public string _password = "";

		public BaseRegion _region;

		public double _summonerID;

		public string _summonerName;

		public double _profileIcon;

		public double _summonerLevel;

		public double _IPBalance;

		public double _RPBalance;

		public ChampionDTO[] _myChampions;

		public bool firstTimeInLobby = true;

		public bool firstTimeInQueuePop = true;

		public bool firstTimeInCustom = true;

		public bool firstTimeInPostChampSelect = true;

		public Process exeProcess;

		public Thread errorThread;

		public LoLConnection _connection = new LoLConnection();

		private TimeSpan t;

		internal exListBoxItem _controllerListItem;

		private System.Timers.Timer dispatcherTimer;

		private string specificFolder;

		private bool _continue;

		private FormWindowState LastWindowState = FormWindowState.Minimized;

		private IContainer components;

		private StatusStrip statusStrip1;

		private TabControl tabControl1;

		private TabPage tabPage1;

		private TabPage tabPage2;

		private PictureBox pictureBox1;

		private GroupBox groupBox1;

		private StatusStrip statusStrip2;

		private RichTextBox richTextBox1;

		private ToolStripDropDownButton toolStripDropDownButton1;

		private ToolStripStatusLabel toolStripStatusLabel2;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private Button button2;

		private Button button1;

		private ComboBox comboBox1;

		private Label label1;

		private ToolStripStatusLabel toolStripStatusLabel3;

		private ComboBox comboBox2;

		private Label label2;

		private GroupBox groupBox3;

		private Label label4;

		private ComboBox comboBox4;

		private Label label3;

		private ComboBox comboBox3;

		private GroupBox groupBox2;

		private CheckBox checkBox1;

		private TabPage tabPage3;

		private Panel lolContainer;

		private TextBox textBox1;

		private TabPage tabPage4;

		private TreeView treeView1;

		private Label label6;

		private Label label8;

		private Label label7;

		private CheckBox checkBox2;

		private Button button3;

		private Button button5;

		private Button button4;

		private Label label5;

		public QueueTypes QueueType
		{
			get;
			set;
		}

		public QueueTypes ActualQueueType
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

		public LoginDataPacket _loginDataPacket
		{
			get;
			set;
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 33554432;
				return createParams;
			}
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		private static extern bool MoveWindow(IntPtr Handle, int x, int y, int w, int h, bool repaint);

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr Handle, int Msg, int wParam, int lParam);

		public vClient(string user, string pass, string region, VoliBot parent, bool autoconnect)
		{
			this._username = user;
			this._password = pass;
			this._parent = parent;
			this._region = BaseRegion.GetRegion(region);
			this.InitializeComponent();
			this.groupBox1.Text = user;
			this.Text = user;
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			this.specificFolder = Path.Combine(folderPath, "VoliBot");
			if (autoconnect)
			{
				this.toolStripStatusLabel1_Click(this, EventArgs.Empty);
			}
		}

		private void toolStripStatusLabel1_Click(object sender, EventArgs e)
		{
			this._connection.OnConnect += new LoLConnection.OnConnectHandler(this.connection_OnConnect);
			this._connection.OnError += new LoLConnection.OnErrorHandler(this.connection_OnError);
			this._connection.OnMessageReceived += new LoLConnection.OnMessageReceivedHandler(this.connection_OnMessageReceived);
			this.updateStatus(msgStatus.INFO, "连接中...");
			this._connection.Connect(this._username, this._password, this._region.PVPRegion, Config.clientSeason + "." + Config.clientSubVersion);
		}

		internal void addListBoxItem(exListBoxItem eLBI)
		{
			this._controllerListItem = eLBI;
		}

		private void toolStripDropDownButton1_Click(object sender, EventArgs e)
		{
			this._connection.Disconnect();
		}

		private void connection_OnConnect(object sender, EventArgs e)
		{
			this._connection.OnDisconnect += new LoLConnection.OnDisconnectHandler(this.connection_OnDisconnect);
			this._connection.OnLogin += new LoLConnection.OnLoginHandler(this.connection_OnLogin);
			this._connection.OnLoginQueueUpdate += new LoLConnection.OnLoginQueueUpdateHandler(this.connection_OnLoginQueueUpdate);
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
					Console.WriteLine("Failed to Start!");
					return;
				case "CHAMP_SELECT":
					this.firstTimeInCustom = true;
					this.firstTimeInQueuePop = true;
					if (!this.firstTimeInLobby)
					{
						return;
					}
					this.firstTimeInLobby = false;
					this.updateStatus(msgStatus.INFO, "In Champion Select");
					base.Invoke(new Action(delegate
					{
						this._parent.onlyUpdateListItemStatusAndLevel(this._controllerListItem, "状态:选择英雄中", this._summonerLevel.ToString());
					}));
					await this._connection.SetClientReceivedGameMessage(gameDTO.Id, "CHAMP_SELECT_CLIENT");
					if (this.QueueType != QueueTypes.ARAM)
					{
						base.Invoke(new Action(delegate
						{
							if (this.comboBox2.Text != "" && this.comboBox2.Text != "RANDOM")
							{
								int spellOneId;
								int spellTwoId;
								if (!this.checkBox1.Checked)
								{
									spellOneId = Enums.spellToId(this.comboBox3.Text.ToUpper());
									spellTwoId = Enums.spellToId(this.comboBox4.Text.ToUpper());
								}
								else
								{
									Random random = new Random();
									List<int> list = new List<int>
									{
										13,
										6,
										7,
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
									int num3 = list[index];
									int num4 = list[index2];
									if (num3 == num4)
									{
										int index3 = random.Next(list.Count);
										num4 = list[index3];
									}
									spellOneId = Convert.ToInt32(num3);
									spellTwoId = Convert.ToInt32(num4);
								}
								await this._connection.SelectSpells(spellOneId, spellTwoId);
								await this._connection.SelectChampion(Enums.championToId(this.comboBox2.Text.ToUpper().Replace(" ", "-")));
								await this._connection.ChampionSelectCompleted();
							}
							else if (this.comboBox2.Text == "RANDOM")
							{
								int spellOneId2;
								int spellTwoId2;
								if (!this.checkBox1.Checked)
								{
									spellOneId2 = Enums.spellToId(this.comboBox3.Text.ToUpper());
									spellTwoId2 = Enums.spellToId(this.comboBox4.Text.ToUpper());
								}
								else
								{
									Random random2 = new Random();
									List<int> list2 = new List<int>
									{
										13,
										6,
										7,
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
									int num5 = list2[index4];
									int num6 = list2[index5];
									if (num5 == num6)
									{
										int index6 = random2.Next(list2.Count);
										num6 = list2[index6];
									}
									spellOneId2 = Convert.ToInt32(num5);
									spellTwoId2 = Convert.ToInt32(num6);
								}
								await this._connection.SelectSpells(spellOneId2, spellTwoId2);
								IEnumerable<ChampionDTO> source = this._myChampions.Shuffle<ChampionDTO>();
								await this._connection.SelectChampion(source.First((ChampionDTO champ) => champ.Owned || champ.FreeToPlay).ChampionId);
								await this._connection.ChampionSelectCompleted();
							}
							else
							{
								int spellOneId3;
								int spellTwoId3;
								if (!this.checkBox1.Checked)
								{
									spellOneId3 = Enums.spellToId(this.comboBox3.Text.ToUpper());
									spellTwoId3 = Enums.spellToId(this.comboBox4.Text.ToUpper());
								}
								else
								{
									Random random3 = new Random();
									List<int> list3 = new List<int>
									{
										13,
										6,
										7,
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
									int num7 = list3[index7];
									int num8 = list3[index8];
									if (num7 == num8)
									{
										int index9 = random3.Next(list3.Count);
										num8 = list3[index9];
									}
									spellOneId3 = Convert.ToInt32(num7);
									spellTwoId3 = Convert.ToInt32(num8);
								}
								await this._connection.SelectSpells(spellOneId3, spellTwoId3);
								await this._connection.SelectChampion(this._myChampions.First((ChampionDTO champ) => champ.Owned || champ.FreeToPlay).ChampionId);
								await this._connection.ChampionSelectCompleted();
							}
						}));
						return;
					}
					return;
				case "POST_CHAMP_SELECT":
					this.firstTimeInLobby = false;
					if (this.firstTimeInPostChampSelect)
					{
						this.firstTimeInPostChampSelect = false;
						this.updateStatus(msgStatus.INFO, "(Post Champ Select)");
						return;
					}
					return;
				case "IN_QUEUE":
					this.updateStatus(msgStatus.INFO, "In Queue");
					base.Invoke(new Action(delegate
					{
						this._parent.onlyUpdateListItemStatusAndLevel(this._controllerListItem, "状态:等待对手", this._summonerLevel.ToString());
					}));
					return;
				case "TERMINATED":
					this.updateStatus(msgStatus.INFO, "Re-entering queue");
					this.firstTimeInPostChampSelect = true;
					this.firstTimeInQueuePop = true;
					return;
				case "JOINING_CHAMP_SELECT":
					if (this.firstTimeInQueuePop && gameDTO.StatusOfParticipants.Contains("1"))
					{
						this.updateStatus(msgStatus.INFO, "Accepted Queue");
						this.firstTimeInQueuePop = false;
						this.firstTimeInLobby = true;
						await this._connection.AcceptPoppedGame(true);
						return;
					}
					return;
				}
				this.updateStatus(msgStatus.INFO, "[DEFAULT]" + gameDTO.GameStateString);
			}
			else if (message.GetType() == typeof(TradeContractDTO))
			{
				TradeContractDTO tradeContractDTO = message as TradeContractDTO;
				if (tradeContractDTO != null)
				{
					string expr_477 = tradeContractDTO.State;
					if (expr_477 != null && expr_477 == "PENDING" && tradeContractDTO != null)
					{
						await this._connection.AcceptTrade(tradeContractDTO.RequesterInternalSummonerName, (int)tradeContractDTO.RequesterChampionId);
					}
				}
			}
			else if (message is PlayerCredentialsDto)
			{
				this.firstTimeInPostChampSelect = true;
				PlayerCredentialsDto playerCredentialsDto = message as PlayerCredentialsDto;
				ProcessStartInfo startInfo = new ProcessStartInfo();
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
				this.updateStatus(msgStatus.INFO, "Launching League of Legends");
				base.Invoke(new Action(delegate
				{
					this._parent.onlyUpdateListItemStatusAndLevel(this._controllerListItem, "状态:开始游戏", this._summonerLevel.ToString());
				}));
				new Thread(delegate
				{
					this.exeProcess = Process.Start(startInfo);
					this.exeProcess.Exited += new EventHandler(this.exeProcess_Exited);
					while (this.exeProcess.MainWindowHandle == IntPtr.Zero)
					{
					}
					this.Invoke(new Action(delegate
					{
						this.button1.Enabled = false;
						if (this.checkBox2.Checked)
						{
							vClient.SetParent(this.exeProcess.MainWindowHandle, this.lolContainer.Handle);
							vClient.MoveWindow(this.exeProcess.MainWindowHandle, 0, 0, this.lolContainer.Width, this.lolContainer.Height, true);
							vClient.SetWindowLong(this.exeProcess.MainWindowHandle, -16, 293601280);
							this.lolContainer.BringToFront();
							this.lolContainer.Resize += new EventHandler(this.moveWindows);
							this.FormClosed += delegate(object sender2, FormClosedEventArgs e)
							{
								vClient.SendMessage(this.exeProcess.MainWindowHandle, 83, 0, 0);
								try
								{
									this.killContainedLeague();
								}
								catch
								{
								}
								Thread.Sleep(1000);
							};
						}
					}));
				}).Start();
			}
			else if (!(message is GameNotification) && !(message is SearchingForMatchNotification))
			{
				if (message is EndOfGameStats)
				{
					if (!this._continue)
					{
						base.Invoke(new Action(delegate
						{
							this.button2.Enabled = false;
							this.button1.Enabled = true;
						}));
					}
					else
					{
						base.Invoke(new Action(delegate
						{
							this.button2.Enabled = true;
						}));
						await this._connection.ackLeaverBusterWarning();
						await this._connection.callPersistenceMessaging(new SimpleDialogMessageResponse
						{
							AccountID = this._loginDataPacket.AllSummonerData.Summoner.SumId,
							MsgID = this._loginDataPacket.AllSummonerData.Summoner.SumId,
							Command = "ack"
						});
						MatchMakerParams matchMakerParams = new MatchMakerParams();
						this.checkAndUpdateQueueType();
						if (this.QueueType == QueueTypes.INTRO_BOT)
						{
							matchMakerParams.BotDifficulty = "INTRO";
						}
						else if (this.QueueType == QueueTypes.BEGINNER_BOT)
						{
							matchMakerParams.BotDifficulty = "EASY";
						}
						else if (this.QueueType == QueueTypes.MEDIUM_BOT)
						{
							matchMakerParams.BotDifficulty = "MEDIUM";
						}
						if (this.QueueType != (QueueTypes)0)
						{
							matchMakerParams.QueueIds = new int[]
							{
								(int)this.QueueType
							};
							SearchingForMatchNotification searchingForMatchNotification = await this._connection.AttachToQueue(matchMakerParams);
							if (searchingForMatchNotification.PlayerJoinFailures == null)
							{
								this.updateStatus(msgStatus.INFO, "In Queue: " + this.QueueType.ToString());
								base.Invoke(new Action(delegate
								{
									this._parent.onlyUpdateListItemStatusAndLevel(this._controllerListItem, "状态:等待对手", this._summonerLevel.ToString());
								}));
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
								}
								if (string.IsNullOrEmpty(this.m_accessToken))
								{
									using (List<QueueDodger>.Enumerator var_27 = searchingForMatchNotification.PlayerJoinFailures.GetEnumerator())
									{
										while (var_27.MoveNext())
										{
											QueueDodger current2 = var_27.Current;
											this.updateStatus(msgStatus.INFO, "Dodge Remaining Time: " + Convert.ToString((float)(current2.DodgePenaltyRemainingTime / 1000) / 60f).Replace(",", ":") + "...");
										}
										return;
									}
								}
								double num2 = (double)((float)(this.m_leaverBustedPenalty / 1000) / 60f);
								this.updateStatus(msgStatus.INFO, "Waiting out leaver buster: " + num2 + " minutes!");
								this.t = TimeSpan.FromMinutes((double)((int)num2));
								this.Tick();
								Thread.Sleep(TimeSpan.FromMilliseconds((double)this.m_leaverBustedPenalty));
								searchingForMatchNotification = await this._connection.AttachToLowPriorityQueue(matchMakerParams, this.m_accessToken);
								if (searchingForMatchNotification.PlayerJoinFailures == null)
								{
									this.updateStatus(msgStatus.INFO, "Succesfully joined lower priority queue!");
								}
								else
								{
									this.updateStatus(msgStatus.ERROR, "There was an error in joining lower priority queue.\nDisconnecting.");
									this._connection.Disconnect();
								}
							}
						}
					}
				}
				else if (message.ToString().Contains("EndOfGameStats"))
				{
					EndOfGameStats message2 = new EndOfGameStats();
					this.connection_OnMessageReceived(sender, message2);
					this.exeProcess.Exited -= new EventHandler(this.exeProcess_Exited);
					this.exeProcess.Kill();
					Thread.Sleep(500);
					if (this.exeProcess.Responding)
					{
						Process.Start("taskkill /F /IM \"League of Legends.exe\"");
					}
				}
			}
		}

		private void connection_OnLogin(object sender, string username, string ipAddress)
		{
			this.updateStatus(msgStatus.INFO, "登陆中...");
			new Thread(delegate
			{
				this._loginDataPacket = await this._connection.GetLoginDataPacketForUser();
				await this._connection.Subscribe("bc", this._loginDataPacket.AllSummonerData.Summoner.AcctId);
				await this._connection.Subscribe("cn", this._loginDataPacket.AllSummonerData.Summoner.AcctId);
				await this._connection.Subscribe("gn", this._loginDataPacket.AllSummonerData.Summoner.AcctId);
				if (this._loginDataPacket.AllSummonerData == null)
				{
					new Random();
					string text = this._username;
					if (text.Length > 16)
					{
						text = text.Substring(0, 12) + new Random().Next(1000, 9999).ToString();
					}
					await this._connection.CreateDefaultSummoner(text);
					this.updateStatus(msgStatus.INFO, "Created Summoner: " + text);
				}
				await this._connection.ackLeaverBusterWarning();
				await this._connection.callPersistenceMessaging(new SimpleDialogMessageResponse
				{
					AccountID = this._loginDataPacket.AllSummonerData.Summoner.SumId,
					MsgID = this._loginDataPacket.AllSummonerData.Summoner.SumId,
					Command = "ack"
				});
				this._summonerID = this._loginDataPacket.AllSummonerData.Summoner.SumId;
				this._summonerName = this._loginDataPacket.AllSummonerData.Summoner.Name;
				this._profileIcon = (double)this._loginDataPacket.AllSummonerData.Summoner.ProfileIconId;
				this._summonerLevel = this._loginDataPacket.AllSummonerData.SummonerLevel.Level;
				this._IPBalance = this._loginDataPacket.IpBalance;
				this._RPBalance = this._loginDataPacket.RpBalance;
				this._myChampions = await this._connection.GetAvailableChampions();
				this.updateStatus(msgStatus.INFO, string.Concat(new object[]
				{
					"Logged in as ",
					this._summonerName,
					" @ level ",
					this._summonerLevel
				}));
				await this._connection.CreatePlayer();
				this.updateSummonerUI();
				if (this._loginDataPacket.ReconnectInfo != null && this._loginDataPacket.ReconnectInfo.Game != null)
				{
					this.connection_OnMessageReceived(sender, this._loginDataPacket.ReconnectInfo.PlayerCredentials);
				}
				this.updateStatusBar("Logged in");
			}).Start();
		}

		private void updateSummonerUI()
		{
			base.Invoke(new Action(delegate
			{
				this.toolStripDropDownButton1.Text = "断开连接";
				this.toolStripDropDownButton1.Image = Resources.disconnect;
				this.toolStripDropDownButton1.Click -= new EventHandler(this.toolStripStatusLabel1_Click);
				this.toolStripDropDownButton1.Click += new EventHandler(this.toolStripDropDownButton1_Click);
				this.toolStripStatusLabel2.Text = "点卷: " + this._RPBalance;
				this.toolStripStatusLabel1.Text = "金币: " + this._IPBalance;
				this.groupBox1.Text = this._summonerName;
				this.comboBox1.Enabled = true;
				this.button1.Enabled = true;
				this.updateTitle(this._summonerName + " | 等级 : " + this._summonerLevel);
				Image newImage = Basic.returnIcon(this._loginDataPacket.AllSummonerData.Summoner.ProfileIconId);
				this._parent.updateListItem(this._controllerListItem, this._summonerName, "状态:已登陆", this._summonerLevel.ToString(), newImage);
				this.getAndChangeIcon(this._loginDataPacket.AllSummonerData.Summoner.ProfileIconId);
				ChampionDTO[] myChampions = this._myChampions;
				for (int i = 0; i < myChampions.Length; i++)
				{
					ChampionDTO championDTO = myChampions[i];
					if (championDTO.Owned)
					{
						this.comboBox2.Items.Add(Basic.UppercaseFirst(Enums.championToString(championDTO.ChampionId)));
					}
					else if (championDTO.FreeToPlay)
					{
						this.comboBox2.Items.Add(Basic.UppercaseFirst(Enums.championToString(championDTO.ChampionId)) + " | [周免]");
						this.comboBox2.Items.Add(Basic.UppercaseFirst(Enums.championToString(championDTO.ChampionId)));
					}
				}
			}));
		}

		private void connection_OnLoginQueueUpdate(object sender, int positionInLine)
		{
			this.updateStatus(msgStatus.INFO, "Login Queue Position: " + positionInLine);
		}

		public void connection_OnDisconnect(object sender, EventArgs e)
		{
			this._connection.OnConnect -= new LoLConnection.OnConnectHandler(this.connection_OnConnect);
			this._connection.OnError -= new LoLConnection.OnErrorHandler(this.connection_OnError);
			this._connection.OnMessageReceived -= new LoLConnection.OnMessageReceivedHandler(this.connection_OnMessageReceived);
			this._connection.OnDisconnect -= new LoLConnection.OnDisconnectHandler(this.connection_OnDisconnect);
			this._connection.OnLogin -= new LoLConnection.OnLoginHandler(this.connection_OnLogin);
			this._connection.OnLoginQueueUpdate -= new LoLConnection.OnLoginQueueUpdateHandler(this.connection_OnLoginQueueUpdate);
			this.updateStatus(msgStatus.INFO, "已断开连接");
			base.Invoke(new Action(delegate
			{
				this.toolStripDropDownButton1.Text = "连接";
				this.toolStripDropDownButton1.Image = Resources.connect;
				this.toolStripDropDownButton1.Click -= new EventHandler(this.toolStripDropDownButton1_Click);
				this.toolStripDropDownButton1.Click += new EventHandler(this.toolStripStatusLabel1_Click);
				this.toolStripStatusLabel2.Text = "RP: 0";
				this.toolStripStatusLabel1.Text = "IP: 0";
				this.groupBox1.Text = this._username;
				this.updateTitle(this._username);
				this.button1.Enabled = false;
				this.button2.Enabled = false;
			}));
		}

		private void exeProcess_Exited(object sender, EventArgs e)
		{
			this.updateStatus(msgStatus.INFO, "重启英雄联盟");
			this.lolContainer.Resize -= new EventHandler(this.moveWindows);
			Thread.Sleep(1000);
			if (this._loginDataPacket.ReconnectInfo != null && this._loginDataPacket.ReconnectInfo.Game != null)
			{
				this.connection_OnMessageReceived(sender, this._loginDataPacket.ReconnectInfo.PlayerCredentials);
				return;
			}
			this.connection_OnMessageReceived(sender, new EndOfGameStats());
		}

		private void connection_OnError(object sender, Error error)
		{
			if (error.Type == ErrorType.AuthKey || error.Type == ErrorType.General)
			{
				Thread thread = new Thread(delegate
				{
					this.updateStatus(msgStatus.INFO, "无法连接，重新连接中...");
				});
				thread.Start();
				return;
			}
			if (error.Message.Contains("召唤师未拥有"))
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
			this.updateStatus(msgStatus.ERROR, error.Message);
		}

		private void updateStatus(msgStatus type, string msg)
		{
			switch (type)
			{
			case msgStatus.ERROR:
				this.richTextBox1.AppendText("[" + type + "]", Color.Red);
				break;
			case msgStatus.INFO:
				this.richTextBox1.AppendText("[" + type + "]", Color.Blue);
				break;
			case msgStatus.DEBUG:
				this.richTextBox1.AppendText("[" + type + "]", Color.Pink);
				break;
			default:
				this.richTextBox1.AppendText("[" + type + "]", Color.Aqua);
				break;
			}
			this.richTextBox1.AppendText("[" + DateTime.Now.ToShortTimeString() + "]", Color.Blue);
			this.richTextBox1.AppendText(" ", Color.Black);
			this.richTextBox1.AppendText(this._username, Color.DarkBlue);
			this.richTextBox1.AppendText(": ", Color.Black);
			this.richTextBox1.AppendText(msg, Color.Black);
			this.richTextBox1.AppendText(Environment.NewLine, Color.Black);
		}

		private void updateStatus(msgStatus type, string msg, Color msgClr)
		{
			switch (type)
			{
			case msgStatus.ERROR:
				this.richTextBox1.AppendText("[" + type + "]", Color.Red);
				break;
			case msgStatus.INFO:
				this.richTextBox1.AppendText("[" + type + "]", Color.Blue);
				break;
			case msgStatus.DEBUG:
				this.richTextBox1.AppendText("[" + type + "]", Color.Pink);
				break;
			default:
				this.richTextBox1.AppendText("[" + type + "]", Color.Aqua);
				break;
			}
			this.richTextBox1.AppendText("[" + DateTime.Now.ToShortTimeString() + "]", Color.Blue);
			this.richTextBox1.AppendText(" ", Color.Black);
			this.richTextBox1.AppendText(this._username, Color.DarkBlue);
			this.richTextBox1.AppendText(": ", Color.Black);
			this.richTextBox1.AppendText(msg, msgClr);
			this.richTextBox1.AppendText(Environment.NewLine, Color.Black);
		}

		private void updateStatusBar(string msg)
		{
			this.toolStripStatusLabel3.Text = "状态: " + msg;
		}

		private void updateTitle(string msg)
		{
			this.Text = msg;
		}

		public void getAndChangeIcon(int id)
		{
			string address = "http://ddragon.leagueoflegends.com/cdn/5.15.1/img/profileicon/" + id.ToString() + ".png";
			string text = this.specificFolder + "\\assets\\icons";
			string text2 = string.Concat(new object[]
			{
				text,
				"\\",
				id,
				".png"
			});
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			if (!File.Exists(text2))
			{
				using (WebClient webClient = new WebClient())
				{
					webClient.DownloadFile(address, text2);
				}
			}
			Image image = Image.FromFile(text2);
			base.Icon = Icon.FromHandle(new Bitmap(image, new Size(16, 16)).GetHicon());
			this.pictureBox1.Image = image;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		public void killContainedLeague()
		{
			IntPtr handle = this.lolContainer.Handle;
			vClient.SendMessage(handle, 16u, IntPtr.Zero, IntPtr.Zero);
		}

		private void vClient_Load(object sender, EventArgs e)
		{
			this.getAndChangeIcon(0);
			object[] queues = Enums.queues;
			for (int i = 0; i < queues.Length; i++)
			{
				string item = (string)queues[i];
				this.comboBox1.Items.Add(item);
			}
			object[] spells = Enums.spells;
			for (int j = 0; j < spells.Length; j++)
			{
				string value = (string)spells[j];
				this.comboBox3.Items.Add(Basic.UppercaseFirst(value));
				this.comboBox4.Items.Add(Basic.UppercaseFirst(value));
			}
			this.comboBox2.Items.Add("RANDOM");
			this.checkLogsAndStats();
		}

		private void checkLogsAndStats()
		{
			string text = this.specificFolder + "\\accounts\\" + this._username;
			new IniFile(text + "\\stats\\" + DateTime.Today.ToString("d") + ".txt");
			if (!File.Exists(text + "\\config.ini"))
			{
				Directory.CreateDirectory(text);
				using (StreamWriter streamWriter = File.CreateText(text + "\\config.ini"))
				{
					streamWriter.Write(string.Concat(new string[]
					{
						"[General]",
						Environment.NewLine,
						"QueueType=",
						Config.defaultQueue,
						Environment.NewLine,
						"LastSpell1=",
						Config.defaultSlotOne,
						Environment.NewLine,
						"LastSpell2=",
						Config.defaultSlotTwo,
						Environment.NewLine,
						"LastChampion=RANDOM",
						Environment.NewLine,
						"LastPath=",
						Config.defaultPath
					}));
				}
				if (!Directory.Exists(text + "\\stats"))
				{
					Directory.CreateDirectory(text + "\\stats");
					if (!File.Exists(text + "\\stats\\" + DateTime.Today.ToString("d") + ".txt"))
					{
						using (StreamWriter streamWriter2 = File.CreateText(text + "\\stats\\" + DateTime.Today.ToString("d") + ".txt"))
						{
							streamWriter2.Write(string.Concat(new string[]
							{
								"[stats]",
								Environment.NewLine,
								"Matches=0",
								Environment.NewLine,
								"Wins=0",
								Environment.NewLine,
								"IP=0",
								Environment.NewLine,
								"XP=0"
							}));
						}
					}
				}
				for (int i = 0; i < this.comboBox1.Items.Count; i++)
				{
					if (this.comboBox1.GetItemText(this.comboBox1.Items[i]) == Config.defaultQueue)
					{
						this.comboBox1.SelectedIndex = i;
					}
				}
				for (int j = 0; j < this.comboBox2.Items.Count; j++)
				{
					if (this.comboBox2.GetItemText(this.comboBox2.Items[j]) == Config.defaultChampion)
					{
						this.comboBox2.SelectedIndex = j;
					}
				}
				for (int k = 0; k < this.comboBox3.Items.Count; k++)
				{
					if (this.comboBox3.GetItemText(this.comboBox3.Items[k]) == Config.defaultSlotOne)
					{
						this.comboBox3.SelectedIndex = k;
					}
				}
				for (int l = 0; l < this.comboBox4.Items.Count; l++)
				{
					if (this.comboBox4.GetItemText(this.comboBox4.Items[l]) == Config.defaultSlotTwo)
					{
						this.comboBox4.SelectedIndex = l;
					}
				}
				this.textBox1.Text = Config.defaultPath;
			}
			else if (File.Exists(text + "\\config.ini"))
			{
				IniFile iniFile = new IniFile(text + "\\config.ini");
				this.textBox1.Text = iniFile.IniReadValue("General", "LastPath");
				for (int m = 0; m < this.comboBox1.Items.Count; m++)
				{
					if (this.comboBox1.GetItemText(this.comboBox1.Items[m]) == iniFile.IniReadValue("General", "QueueType"))
					{
						this.comboBox1.SelectedIndex = m;
					}
				}
				for (int n = 0; n < this.comboBox2.Items.Count; n++)
				{
					if (this.comboBox2.GetItemText(this.comboBox2.Items[n]) == iniFile.IniReadValue("General", "LastChampion"))
					{
						this.comboBox2.SelectedIndex = n;
					}
				}
				for (int num = 0; num < this.comboBox3.Items.Count; num++)
				{
					if (this.comboBox3.GetItemText(this.comboBox3.Items[num]) == iniFile.IniReadValue("General", "LastSpell1"))
					{
						this.comboBox3.SelectedIndex = num;
					}
				}
				for (int num2 = 0; num2 < this.comboBox4.Items.Count; num2++)
				{
					if (this.comboBox4.GetItemText(this.comboBox4.Items[num2]) == iniFile.IniReadValue("General", "LastSpell2"))
					{
						this.comboBox4.SelectedIndex = num2;
					}
				}
			}
			string[] files = Directory.GetFiles(text + "\\stats\\", "*.txt");
			for (int num3 = 0; num3 < files.Length; num3++)
			{
				string text2 = files[num3];
				try
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text2);
					IniFile iniFile2 = new IniFile(text2);
					TreeNode treeNode = new TreeNode();
					TreeNode treeNode2 = new TreeNode();
					TreeNode treeNode3 = new TreeNode();
					TreeNode treeNode4 = new TreeNode();
					TreeNode treeNode5 = new TreeNode();
					TreeNode treeNode6 = new TreeNode();
					treeNode.Text = fileNameWithoutExtension;
					treeNode2.Text = "场次:" + iniFile2.IniReadValue("Stats", "Matches");
					treeNode3.Text = "胜场:" + iniFile2.IniReadValue("Stats", "Wins");
					treeNode4.Text = "输场" + (Convert.ToInt32(iniFile2.IniReadValue("Stats", "Matches")) - Convert.ToInt32(iniFile2.IniReadValue("Stats", "Wins")));
					treeNode5.Text = "获得金币:" + iniFile2.IniReadValue("Stats", "IP");
					treeNode6.Text = "获得点卷:" + iniFile2.IniReadValue("Stats", "XP");
					this.treeView1.Nodes.Add(treeNode);
					treeNode.Nodes.Add(treeNode2);
					treeNode.Nodes.Add(treeNode3);
					treeNode.Nodes.Add(treeNode4);
					treeNode.Nodes.Add(treeNode5);
					treeNode.Nodes.Add(treeNode6);
				}
				catch
				{
				}
			}
		}

		private void checkAndUpdateQueueType()
		{
			base.Invoke(new Action(delegate
			{
				try
				{
					if (this.comboBox1.Text != "")
					{
						if (this.comboBox1.Text.ToString().Contains("5vs5"))
						{
							this.QueueType = QueueTypes.NORMAL_5x5;
						}
						else if (this.comboBox1.Text.ToString().Contains("3vs3"))
						{
							this.QueueType = QueueTypes.NORMAL_3x3;
						}
						else if (this.comboBox1.Text.ToString().Contains("ARAM"))
						{
							this.QueueType = QueueTypes.ARAM;
						}
						else if (this.comboBox1.Text.ToString().Contains("Intro"))
						{
							this.QueueType = QueueTypes.INTRO_BOT;
						}
						else if (this.comboBox1.Text.ToString().Contains("Beginner"))
						{
							this.QueueType = QueueTypes.BEGINNER_BOT;
						}
						else if (this.comboBox1.Text.ToString().Contains("Intermediate"))
						{
							this.QueueType = QueueTypes.MEDIUM_BOT;
						}
					}
					else
					{
						this.updateStatus(msgStatus.ERROR, "你必须选择一个游戏模式");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}));
			try
			{
				if (this._summonerLevel < 3.0 && this.QueueType == QueueTypes.NORMAL_5x5)
				{
					this.updateStatus(msgStatus.INFO, "需要等级3才能进入 NORMAL_5x5游戏模式");
					this.updateStatus(msgStatus.INFO, "将进行人机至等级3");
					this.QueueType = QueueTypes.BEGINNER_BOT;
					this.ActualQueueType = QueueTypes.NORMAL_5x5;
				}
				else if (this._summonerLevel < 6.0 && this.QueueType == QueueTypes.ARAM)
				{
					this.updateStatus(msgStatus.INFO, "需要等级6才能进入ARAM游戏模式");
					this.updateStatus(msgStatus.INFO, "将进行人机至等级6");
					this.QueueType = QueueTypes.BEGINNER_BOT;
					this.ActualQueueType = QueueTypes.ARAM;
				}
				else if (this._summonerLevel < 7.0 && this.QueueType == QueueTypes.NORMAL_3x3)
				{
					this.updateStatus(msgStatus.INFO, "需要等级7才能进入 NORMAL_3x3 游戏模式");
					this.updateStatus(msgStatus.INFO, "将进行人机至等级7");
					this.QueueType = QueueTypes.BEGINNER_BOT;
					this.ActualQueueType = QueueTypes.NORMAL_3x3;
				}
				if (this._summonerLevel == 3.0 && this.ActualQueueType == QueueTypes.NORMAL_5x5)
				{
					this.QueueType = this.ActualQueueType;
				}
				else if (this._summonerLevel == 6.0 && this.ActualQueueType == QueueTypes.ARAM)
				{
					this.QueueType = this.ActualQueueType;
				}
				else if (this._summonerLevel == 7.0 && this.ActualQueueType == QueueTypes.NORMAL_3x3)
				{
					this.QueueType = this.ActualQueueType;
				}
			}
			catch (Exception)
			{
				MessageBox.Show("^2");
			}
		}

		private async void buyBoost()
		{
			try
			{
				string requestUri = await this._connection.GetStoreUrl();
				HttpClient httpClient = new HttpClient();
				await httpClient.GetStringAsync(requestUri);
				string requestUri2 = "https://store." + this._region.ChatName + ".lol.riotgames.com/store/tabs/view/boosts/1";
				await httpClient.GetStringAsync(requestUri2);
				string requestUri3 = "https://store." + this._region.ChatName + ".lol.riotgames.com/store/purchase/item";
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
				this.updateStatus(msgStatus.INFO, "Bought 'XP Boost: 3 Days'!");
				httpClient.Dispose();
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		private string FindLoLExe()
		{
			string result;
			try
			{
				if (!this.textBox1.Text.EndsWith("\\"))
				{
					this.textBox1.Text = this.textBox1.Text + "\\";
				}
				string text = this.textBox1.Text;
				if (text.Contains("notfound"))
				{
					result = text;
				}
				else
				{
					text += "RADS\\solutions\\lol_game_client_sln\\releases\\";
					text = (from f in Directory.EnumerateDirectories(text)
					orderby new DirectoryInfo(f).CreationTime
					select f).Last<string>();
					text += "\\deploy\\";
					result = text;
				}
			}
			catch (DirectoryNotFoundException)
			{
				result = "";
			}
			return result;
		}

		private void vClient_FormClosing(object sender, FormClosingEventArgs e)
		{
			this._parent.removeMdiChild(this._username, this._controllerListItem);
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBox2.Checked && this.exeProcess != null)
			{
				IntPtr arg_22_0 = this.lolContainer.Handle;
			}
		}

		public void moveWindows(object sender2, EventArgs ve)
		{
			vClient.MoveWindow(this.exeProcess.MainWindowHandle, 0, 0, this.lolContainer.Width, this.lolContainer.Height, true);
		}

		private void vClient_Resize(object sender, EventArgs e)
		{
			if (base.WindowState != this.LastWindowState)
			{
				this.LastWindowState = base.WindowState;
				if (base.WindowState == FormWindowState.Maximized)
				{
					base.ShowIcon = true;
				}
				if (base.WindowState == FormWindowState.Normal)
				{
					base.ShowIcon = false;
				}
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "lol.launcher.exe|lol.launcher.exe";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				this.textBox1.Text = Path.GetDirectoryName(fileName) + "\\";
				return;
			}
		}

		private void vClient_SizeChanged(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.tabControl1.SelectTab(0);
			this._continue = true;
			if (!File.Exists(this.FindLoLExe() + "League of Legends.exe"))
			{
				this.updateStatus(msgStatus.ERROR, "未找到英雄联盟地址! 请在设置中修改游戏地址!~");
				return;
			}
			this.updateStatus(msgStatus.INFO, "定位到英雄联盟", Color.Green);
			new Thread(delegate
			{
				await this._connection.ackLeaverBusterWarning();
				await this._connection.callPersistenceMessaging(new SimpleDialogMessageResponse
				{
					AccountID = this._loginDataPacket.AllSummonerData.Summoner.SumId,
					MsgID = this._loginDataPacket.AllSummonerData.Summoner.SumId,
					Command = "ack"
				});
				MatchMakerParams matchMakerParams = new MatchMakerParams();
				this.checkAndUpdateQueueType();
				if (this.QueueType == QueueTypes.INTRO_BOT)
				{
					matchMakerParams.BotDifficulty = "INTRO";
				}
				else if (this.QueueType == QueueTypes.BEGINNER_BOT)
				{
					matchMakerParams.BotDifficulty = "EASY";
				}
				else if (this.QueueType == QueueTypes.MEDIUM_BOT)
				{
					matchMakerParams.BotDifficulty = "MEDIUM";
				}
				this.updateStatus(msgStatus.INFO, this.QueueType.ToString());
				if (this.QueueType != (QueueTypes)0)
				{
					matchMakerParams.QueueIds = new int[]
					{
						(int)this.QueueType
					};
					SearchingForMatchNotification searchingForMatchNotification = await this._connection.AttachToQueue(matchMakerParams);
					if (searchingForMatchNotification.PlayerJoinFailures == null)
					{
						this.updateStatus(msgStatus.INFO, "队列中: " + this.QueueType.ToString());
						base.Invoke(new Action(delegate
						{
							this.button1.Enabled = false;
							this.button2.Enabled = true;
						}));
					}
					else
					{
						foreach (QueueDodger current in searchingForMatchNotification.PlayerJoinFailures)
						{
							if (current.ReasonFailed == "挂及惩罚！")
							{
								this.m_accessToken = current.AccessToken;
								if (current.LeaverPenaltyMillisRemaining > this.m_leaverBustedPenalty)
								{
									this.m_leaverBustedPenalty = current.LeaverPenaltyMillisRemaining;
								}
							}
						}
						if (string.IsNullOrEmpty(this.m_accessToken))
						{
							using (List<QueueDodger>.Enumerator var_14 = searchingForMatchNotification.PlayerJoinFailures.GetEnumerator())
							{
								while (var_14.MoveNext())
								{
									QueueDodger current2 = var_14.Current;
									this.updateStatus(msgStatus.INFO, "挂机惩罚剩余时间" + Convert.ToString((float)(current2.DodgePenaltyRemainingTime / 1000) / 60f).Replace(",", ":") + "...");
								}
								return;
							}
						}
						double num = (double)((float)(this.m_leaverBustedPenalty / 1000) / 60f);
						this.updateStatus(msgStatus.INFO, "等待惩罚时间中: " + num + "分钟!");
						this.t = TimeSpan.FromMinutes((double)((int)num));
						this.Tick();
						Thread.Sleep(TimeSpan.FromMilliseconds((double)this.m_leaverBustedPenalty));
						searchingForMatchNotification = await this._connection.AttachToLowPriorityQueue(matchMakerParams, this.m_accessToken);
						if (searchingForMatchNotification.PlayerJoinFailures == null)
						{
							this.updateStatus(msgStatus.INFO, "成功加入低级优先队列中");
						}
						else
						{
							this.updateStatus(msgStatus.ERROR, "无法加入加入低级优先队列，断开连接");
							this._connection.Disconnect();
						}
					}
				}
			}).Start();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this._continue = false;
			base.Invoke(new Action(delegate
			{
				bool flag = await this._connection.CancelFromQueueIfPossible((int)this.QueueType);
				if (flag)
				{
					this.updateStatus(msgStatus.INFO, "已离开队列");
					this.button1.Enabled = true;
					this.button2.Enabled = false;
				}
				else
				{
					this.updateStatus(msgStatus.INFO, "无法离开队列");
				}
			}));
		}

		public void Tick()
		{
			this.dispatcherTimer = new System.Timers.Timer(1000.0);
			this.dispatcherTimer.Elapsed += new ElapsedEventHandler(this.dispatcherTimer_Tick);
			this.dispatcherTimer.Interval = 1000.0;
			this.dispatcherTimer.Start();
		}

		private void dispatcherTimer_Tick(object sender, ElapsedEventArgs e)
		{
			this.t = this.t.Subtract(TimeSpan.FromSeconds(1.0));
			if (this.t.Seconds < 0)
			{
				base.Invoke(new Action(delegate
				{
					this._parent.onlyUpdateListItemStatusAndLevel(this._controllerListItem, "状态:等待对手", this._summonerLevel.ToString());
				}));
				this.dispatcherTimer.Stop();
				return;
			}
			int num = 0;
			int num2 = 0;
			string text = this.t.Minutes.ToString();
			for (int i = 0; i < text.Length; i++)
			{
				char arg_88_0 = text[i];
				num++;
			}
			string text2 = this.t.Seconds.ToString();
			for (int j = 0; j < text2.Length; j++)
			{
				char arg_C2_0 = text2[j];
				num2++;
			}
			string minutes = this.t.Minutes.ToString();
			string seconds = this.t.Seconds.ToString();
			if (num == 1)
			{
				minutes = "0" + this.t.Minutes;
			}
			if (num2 == 1)
			{
				seconds = "0" + this.t.Seconds;
			}
			base.Invoke(new Action(delegate
			{
				this._parent.onlyUpdateListItemStatusAndLevel(this._controllerListItem, "状态:等待" + minutes + ":" + seconds, this._summonerLevel.ToString());
			}));
		}

		private void label5_Click(object sender, EventArgs e)
		{
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.QueueType = (QueueTypes)Enum.Parse(typeof(QueueTypes), this.comboBox1.Text);
			string str = this.specificFolder + "\\accounts\\" + this._username;
			IniFile iniFile = new IniFile(str + "\\config.ini");
			iniFile.IniWriteValue("General", "QueueType", this.comboBox1.Text);
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			string str = this.specificFolder + "\\accounts\\" + this._username;
			IniFile iniFile = new IniFile(str + "\\config.ini");
			iniFile.IniWriteValue("General", "LastChampion", this.comboBox2.Text);
		}

		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			string str = this.specificFolder + "\\accounts\\" + this._username;
			IniFile iniFile = new IniFile(str + "\\config.ini");
			iniFile.IniWriteValue("General", "LastSpell1", this.comboBox3.Text);
			iniFile.IniWriteValue("General", "LastSpell2", this.comboBox4.Text);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			string str = this.specificFolder + "\\accounts\\" + this._username;
			IniFile iniFile = new IniFile(str + "\\config.ini");
			iniFile.IniWriteValue("General", "LastPath", this.textBox1.Text);
		}

		private void statusStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
		}

		private void button4_Click(object sender, EventArgs e)
		{
			Basic.ReplaceGameConfig(this.textBox1.Text);
			this.updateStatus(msgStatus.INFO, "小窗模式开启（删除League of Legends\\Config下game.cfg恢复)");
		}

		private void button5_Click(object sender, EventArgs e)
		{
			Basic.DeleteGameConfig(this.textBox1.Text);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel3 = new ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new TabPage();
			this.richTextBox1 = new RichTextBox();
			this.tabPage4 = new TabPage();
			this.treeView1 = new TreeView();
			this.tabPage2 = new TabPage();
			this.button3 = new Button();
			this.label6 = new Label();
			this.textBox1 = new TextBox();
			this.groupBox3 = new GroupBox();
			this.checkBox1 = new CheckBox();
			this.label4 = new Label();
			this.comboBox4 = new ComboBox();
			this.label3 = new Label();
			this.comboBox3 = new ComboBox();
			this.groupBox2 = new GroupBox();
			this.label2 = new Label();
			this.comboBox2 = new ComboBox();
			this.tabPage3 = new TabPage();
			this.lolContainer = new Panel();
			this.label8 = new Label();
			this.label7 = new Label();
			this.checkBox2 = new CheckBox();
			this.groupBox1 = new GroupBox();
			this.comboBox1 = new ComboBox();
			this.label1 = new Label();
			this.button2 = new Button();
			this.button1 = new Button();
			this.statusStrip2 = new StatusStrip();
			this.toolStripDropDownButton1 = new ToolStripDropDownButton();
			this.pictureBox1 = new PictureBox();
			this.button4 = new Button();
			this.label5 = new Label();
			this.button5 = new Button();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.lolContainer.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.statusStrip2.SuspendLayout();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel3,
				this.toolStripStatusLabel2,
				this.toolStripStatusLabel1
			});
			this.statusStrip1.Location = new Point(0, 315);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(520, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
			this.toolStripStatusLabel3.Size = new Size(411, 17);
			this.toolStripStatusLabel3.Spring = true;
			this.toolStripStatusLabel3.Text = "状态:等待中";
			this.toolStripStatusLabel2.Image = Resources.bumb;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.RightToLeft = RightToLeft.No;
			this.toolStripStatusLabel2.Size = new Size(49, 17);
			this.toolStripStatusLabel2.Text = "点卷: 0";
			this.toolStripStatusLabel1.Image = Resources.sword;
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(45, 17);
			this.toolStripStatusLabel1.Text = "金币: 0";
			this.tabControl1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Location = new Point(12, 95);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(496, 217);
			this.tabControl1.TabIndex = 3;
			this.tabPage1.Controls.Add(this.richTextBox1);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(488, 191);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "状态";
			this.tabPage1.UseVisualStyleBackColor = true;
			this.richTextBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.richTextBox1.BackColor = SystemColors.Window;
			this.richTextBox1.BorderStyle = BorderStyle.None;
			this.richTextBox1.Location = new Point(6, 6);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new Size(476, 179);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			this.tabPage4.Controls.Add(this.treeView1);
			this.tabPage4.Location = new Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new Padding(3);
			this.tabPage4.Size = new Size(488, 191);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "統計";
			this.tabPage4.UseVisualStyleBackColor = true;
			this.treeView1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.treeView1.Location = new Point(6, 6);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new Size(476, 182);
			this.treeView1.TabIndex = 0;
			this.tabPage2.Controls.Add(this.button5);
			this.tabPage2.Controls.Add(this.button4);
			this.tabPage2.Controls.Add(this.button3);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.textBox1);
			this.tabPage2.Controls.Add(this.groupBox3);
			this.tabPage2.Controls.Add(this.groupBox2);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(488, 191);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "选项";
			this.tabPage2.UseVisualStyleBackColor = true;
			this.button3.Location = new Point(225, 83);
			this.button3.Name = "button3";
			this.button3.Size = new Size(26, 23);
			this.button3.TabIndex = 7;
			this.button3.Text = "...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.label6.AutoSize = true;
			this.label6.Location = new Point(6, 72);
			this.label6.Name = "label6";
			this.label6.Size = new Size(77, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "英雄联盟目录:";
			this.textBox1.BorderStyle = BorderStyle.FixedSingle;
			this.textBox1.Location = new Point(9, 85);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(214, 20);
			this.textBox1.TabIndex = 6;
			this.textBox1.Text = "C:\\Riot Games\\League of Legends\\";
			this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
			this.groupBox3.Controls.Add(this.checkBox1);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.comboBox4);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.comboBox3);
			this.groupBox3.Location = new Point(261, 6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new Size(217, 99);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "技能";
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(20, 69);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(101, 17);
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "随机技能 ?";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(114, 16);
			this.label4.Name = "label4";
			this.label4.Size = new Size(37, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "技能2:";
			this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Location = new Point(117, 32);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new Size(81, 21);
			this.comboBox4.TabIndex = 5;
			this.comboBox4.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(17, 16);
			this.label3.Name = "label3";
			this.label3.Size = new Size(37, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "技能 1:";
			this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new Point(20, 32);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new Size(81, 21);
			this.comboBox3.TabIndex = 3;
			this.comboBox3.SelectedIndexChanged += new EventHandler(this.comboBox3_SelectedIndexChanged);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.comboBox2);
			this.groupBox2.Location = new Point(6, 6);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(249, 63);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "英雄";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(6, 16);
			this.label2.Name = "label2";
			this.label2.Size = new Size(93, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "选择英雄:";
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new Point(9, 33);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new Size(123, 21);
			this.comboBox2.TabIndex = 1;
			this.comboBox2.SelectedIndexChanged += new EventHandler(this.comboBox2_SelectedIndexChanged);
			this.tabPage3.BackColor = Color.DimGray;
			this.tabPage3.Controls.Add(this.lolContainer);
			this.tabPage3.Location = new Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new Padding(3);
			this.tabPage3.Size = new Size(488, 191);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "游戏状态（未测试）";
			this.lolContainer.BackColor = Color.Transparent;
			this.lolContainer.Controls.Add(this.label8);
			this.lolContainer.Controls.Add(this.label7);
			this.lolContainer.Controls.Add(this.checkBox2);
			this.lolContainer.Dock = DockStyle.Fill;
			this.lolContainer.Location = new Point(3, 3);
			this.lolContainer.Name = "lolContainer";
			this.lolContainer.Size = new Size(482, 185);
			this.lolContainer.TabIndex = 0;
			this.label8.AutoSize = true;
			this.label8.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label8.ForeColor = SystemColors.ControlLight;
			this.label8.Location = new Point(16, 29);
			this.label8.Name = "label8";
			this.label8.Size = new Size(304, 45);
			this.label8.TabIndex = 1;
			this.label8.Text = "开启后可以选择正在挂机中的某一个账号来进行观看。";
			this.label7.AutoSize = true;
			this.label7.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label7.ForeColor = SystemColors.ControlLight;
			this.label7.Location = new Point(16, 11);
			this.label7.Name = "label7";
			this.label7.Size = new Size(93, 18);
			this.label7.TabIndex = 0;
			this.label7.Text = "这是什么？";
			this.checkBox2.AutoSize = true;
			this.checkBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
			this.checkBox2.ForeColor = Color.Snow;
			this.checkBox2.Location = new Point(19, 86);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new Size(205, 17);
			this.checkBox2.TabIndex = 2;
			this.checkBox2.Text = "为这个客户端使用这个功能？（我没测试，最好别用）";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new EventHandler(this.checkBox2_CheckedChanged);
			this.groupBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Location = new Point(82, 25);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(426, 64);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			this.comboBox1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.Enabled = false;
			this.comboBox1.Location = new Point(196, 35);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(100, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
			this.label1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(193, 19);
			this.label1.Name = "label1";
			this.label1.Size = new Size(69, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "游戏选项:";
			this.button2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.button2.Enabled = false;
			this.button2.Location = new Point(367, 19);
			this.button2.Name = "button2";
			this.button2.Size = new Size(49, 39);
			this.button2.TabIndex = 1;
			this.button2.Text = "停止";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.button1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.button1.Enabled = false;
			this.button1.Location = new Point(302, 19);
			this.button1.Name = "button1";
			this.button1.Size = new Size(59, 39);
			this.button1.TabIndex = 0;
			this.button1.Text = "开始咯♥";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.statusStrip2.BackColor = SystemColors.ControlLight;
			this.statusStrip2.Dock = DockStyle.Top;
			this.statusStrip2.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripDropDownButton1
			});
			this.statusStrip2.Location = new Point(0, 0);
			this.statusStrip2.Name = "statusStrip2";
			this.statusStrip2.RenderMode = ToolStripRenderMode.Professional;
			this.statusStrip2.Size = new Size(520, 22);
			this.statusStrip2.SizingGrip = false;
			this.statusStrip2.TabIndex = 8;
			this.statusStrip2.Text = "statusStrip2";
			this.statusStrip2.ItemClicked += new ToolStripItemClickedEventHandler(this.statusStrip2_ItemClicked);
			this.toolStripDropDownButton1.Image = Resources.connect;
			this.toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.ShowDropDownArrow = false;
			this.toolStripDropDownButton1.Size = new Size(72, 20);
			this.toolStripDropDownButton1.Text = "连接账号";
			this.toolStripDropDownButton1.Click += new EventHandler(this.toolStripStatusLabel1_Click);
			this.pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
			this.pictureBox1.ImageLocation = "";
			this.pictureBox1.Location = new Point(12, 25);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(64, 64);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			this.button4.Location = new Point(6, 111);
			this.button4.Name = "button4";
			this.button4.Size = new Size(132, 23);
			this.button4.TabIndex = 8;
			this.button4.Text = "替换CONFIG（小窗）";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new EventHandler(this.button4_Click);
			this.label5.AutoSize = true;
			this.label5.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label5.Location = new Point(14, 133);
			this.label5.Name = "label5";
			this.label5.Size = new Size(231, 12);
			this.label5.TabIndex = 9;
			this.label5.Text = "变成小窗后想恢复就点删除config，会手动有备份的无视即可";
			this.button5.Location = new Point(144, 112);
			this.button5.Name = "button5";
			this.button5.Size = new Size(107, 23);
			this.button5.TabIndex = 10;
			this.button5.Text = "删除CONFIG";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new EventHandler(this.button5_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(520, 337);
			base.Controls.Add(this.statusStrip2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.MinimumSize = new Size(350, 240);
			base.Name = "vClient";
			base.ShowIcon = false;
			base.StartPosition = FormStartPosition.WindowsDefaultBounds;
			this.Text = "vClient";
			base.FormClosing += new FormClosingEventHandler(this.vClient_FormClosing);
			base.Load += new EventHandler(this.vClient_Load);
			base.SizeChanged += new EventHandler(this.vClient_SizeChanged);
			base.Resize += new EventHandler(this.vClient_Resize);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.lolContainer.ResumeLayout(false);
			this.lolContainer.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.statusStrip2.ResumeLayout(false);
			this.statusStrip2.PerformLayout();
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
