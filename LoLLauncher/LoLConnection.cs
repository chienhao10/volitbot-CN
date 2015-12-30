using LoLLauncher.RiotObjects;
using LoLLauncher.RiotObjects.Leagues.Pojo;
using LoLLauncher.RiotObjects.Platform.Catalog.Champion;
using LoLLauncher.RiotObjects.Platform.Clientfacade.Domain;
using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Game.Message;
using LoLLauncher.RiotObjects.Platform.Game.Practice;
using LoLLauncher.RiotObjects.Platform.Harassment;
using LoLLauncher.RiotObjects.Platform.Leagues.Client.Dto;
using LoLLauncher.RiotObjects.Platform.Login;
using LoLLauncher.RiotObjects.Platform.Matchmaking;
using LoLLauncher.RiotObjects.Platform.Messaging;
using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
using LoLLauncher.RiotObjects.Platform.Statistics;
using LoLLauncher.RiotObjects.Platform.Statistics.Team;
using LoLLauncher.RiotObjects.Platform.Summoner;
using LoLLauncher.RiotObjects.Platform.Summoner.Boost;
using LoLLauncher.RiotObjects.Platform.Summoner.Masterybook;
using LoLLauncher.RiotObjects.Platform.Summoner.Runes;
using LoLLauncher.RiotObjects.Platform.Summoner.Spellbook;
using LoLLauncher.RiotObjects.Team;
using LoLLauncher.RiotObjects.Team.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VoliBot.LoLLauncher.RiotObjects.Platform.Messaging;

namespace LoLLauncher
{
	public class LoLConnection
	{
		public delegate void OnConnectHandler(object sender, EventArgs e);

		public delegate void OnLoginQueueUpdateHandler(object sender, int positionInLine);

		public delegate void OnLoginHandler(object sender, string username, string ipAddress);

		public delegate void OnDisconnectHandler(object sender, EventArgs e);

		public delegate void OnMessageReceivedHandler(object sender, object message);

		public delegate void OnErrorHandler(object sender, Error error);

		private bool isConnected;

		private bool isLoggedIn;

		private TcpClient client;

		private SslStream sslStream;

		private string ipAddress;

		private string authToken;

		private int accountID;

		private string sessionToken;

		private string DSId;

		private string user;

		private string password;

		private string server;

		private string loginQueue;

		private string locale;

		private string clientVersion;

		private bool useGarena;

		private string garenaToken;

		private string userID;

		private Random rand = new Random();

		private JavaScriptSerializer serializer = new JavaScriptSerializer();

		private int invokeID = 2;

		private List<int> pendingInvokes = new List<int>();

		private Dictionary<int, TypedObject> results = new Dictionary<int, TypedObject>();

		private Dictionary<int, RiotGamesObject> callbacks = new Dictionary<int, RiotGamesObject>();

		public Thread decodeThread;

		private int heartbeatCount = 1;

		public Thread heartbeatThread;

		private object isInvokingLock = new object();

		public event LoLConnection.OnConnectHandler OnConnect;

		public event LoLConnection.OnLoginQueueUpdateHandler OnLoginQueueUpdate;

		public event LoLConnection.OnLoginHandler OnLogin;

		public event LoLConnection.OnDisconnectHandler OnDisconnect;

		public event LoLConnection.OnMessageReceivedHandler OnMessageReceived;

		public event LoLConnection.OnErrorHandler OnError;

		public void Connect(string user, string password, RegioN region, string clientVersion)
		{
			if (!this.isConnected)
			{
				Thread thread = new Thread(delegate
				{
					this.user = user;
					this.password = password;
					this.clientVersion = clientVersion;
					this.server = RegionInfo.GetServerValue(region);
					this.loginQueue = RegionInfo.GetLoginQueueValue(region);
					this.locale = RegionInfo.GetLocaleValue(region);
					this.useGarena = RegionInfo.GetUseGarenaValue(region);
					try
					{
						this.client = new TcpClient(this.server, 2099);
					}
					catch
					{
						this.Error("Riots servers are currently unavailable.", ErrorType.AuthKey);
						this.Disconnect();
						return;
					}
					if (!this.useGarena || this.GetGarenaToken())
					{
						if (!this.GetAuthKey())
						{
							return;
						}
						if (!this.GetIpAddress())
						{
							return;
						}
						this.sslStream = new SslStream(this.client.GetStream(), false, new RemoteCertificateValidationCallback(this.AcceptAllCertificates));
						IAsyncResult asyncResult = this.sslStream.BeginAuthenticateAsClient(this.server, null, null);
						using (asyncResult.AsyncWaitHandle)
						{
							if (asyncResult.AsyncWaitHandle.WaitOne(-1))
							{
								this.sslStream.EndAuthenticateAsClient(asyncResult);
							}
						}
						if (!this.Handshake())
						{
							return;
						}
						this.BeginReceive();
						if (!this.SendConnect())
						{
							return;
						}
						if (!this.Login())
						{
							return;
						}
						this.StartHeartbeat();
						return;
					}
				});
				thread.Start();
			}
		}

		private bool AcceptAllCertificates(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		private bool GetGarenaToken()
		{
			this.Error("Garena Servers are not yet supported", ErrorType.Login);
			this.Disconnect();
			return false;
		}

		private bool GetAuthKey()
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				string str = "user=" + this.user + ",password=" + this.password;
				string s = "payload=" + str;
				if (this.useGarena)
				{
					str = this.garenaToken;
				}
				WebRequest webRequest = WebRequest.Create(this.loginQueue + "login-queue/rest/queue/authenticate");
				webRequest.Method = "POST";
				Stream requestStream = webRequest.GetRequestStream();
				requestStream.Write(Encoding.ASCII.GetBytes(s), 0, Encoding.ASCII.GetByteCount(s));
				WebResponse response = webRequest.GetResponse();
				Stream responseStream = response.GetResponseStream();
				int num;
				while ((num = responseStream.ReadByte()) != -1)
				{
					stringBuilder.Append((char)num);
				}
				TypedObject typedObject = this.serializer.Deserialize<TypedObject>(stringBuilder.ToString());
				requestStream.Close();
				responseStream.Close();
				webRequest.Abort();
				if (!typedObject.ContainsKey("token"))
				{
					int value = typedObject.GetInt("node").Value;
					string @string = typedObject.GetString("champ");
					int value2 = typedObject.GetInt("rate").Value;
					int value3 = typedObject.GetInt("delay").Value;
					int num2 = 0;
					int num3 = 0;
					object[] array = typedObject.GetArray("tickers");
					object[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						object obj = array2[i];
						Dictionary<string, object> dictionary = (Dictionary<string, object>)obj;
						int num4 = (int)dictionary["node"];
						if (num4 == value)
						{
							num2 = (int)dictionary["id"];
							num3 = (int)dictionary["current"];
							IL_26B:
							while (num2 - num3 > value2)
							{
								stringBuilder.Clear();
								if (this.OnLoginQueueUpdate != null)
								{
									this.OnLoginQueueUpdate(this, num2 - num3);
								}
								Thread.Sleep(value3);
								webRequest = WebRequest.Create(this.loginQueue + "login-queue/rest/queue/ticker/" + @string);
								webRequest.Method = "GET";
								response = webRequest.GetResponse();
								responseStream = response.GetResponseStream();
								int num5;
								while ((num5 = responseStream.ReadByte()) != -1)
								{
									stringBuilder.Append((char)num5);
								}
								typedObject = this.serializer.Deserialize<TypedObject>(stringBuilder.ToString());
								responseStream.Close();
								webRequest.Abort();
								if (typedObject != null)
								{
									num3 = this.HexToInt(typedObject.GetString(value.ToString()));
								}
							}
							while (stringBuilder.ToString() == null || !typedObject.ContainsKey("token"))
							{
								try
								{
									stringBuilder.Clear();
									if (num2 - num3 < 0)
									{
										if (this.OnLoginQueueUpdate != null)
										{
											this.OnLoginQueueUpdate(this, 0);
										}
										else if (this.OnLoginQueueUpdate != null)
										{
											this.OnLoginQueueUpdate(this, num2 - num3);
										}
									}
									Thread.Sleep(value3 / 10);
									webRequest = WebRequest.Create(this.loginQueue + "login-queue/rest/queue/authToken/" + this.user.ToLower());
									webRequest.Method = "GET";
									response = webRequest.GetResponse();
									responseStream = response.GetResponseStream();
									int num6;
									while ((num6 = responseStream.ReadByte()) != -1)
									{
										stringBuilder.Append((char)num6);
									}
									typedObject = this.serializer.Deserialize<TypedObject>(stringBuilder.ToString());
									responseStream.Close();
									webRequest.Abort();
								}
								catch
								{
								}
							}
							goto IL_35C;
						}
					}
					goto IL_26B;
				}
				IL_35C:
				if (this.OnLoginQueueUpdate != null)
				{
					this.OnLoginQueueUpdate(this, 0);
				}
				this.authToken = typedObject.GetString("token");
				result = true;
			}
			catch (Exception ex)
			{
				if (ex.Message == "The remote name could not be resolved: '" + this.loginQueue + "'")
				{
					this.Error("Please make sure you are connected the internet!", ErrorType.AuthKey);
					this.Disconnect();
				}
				else if (ex.Message == "The remote server returned an error: (403) Forbidden.")
				{
					this.Error("Your username or password is incorrect!", ErrorType.Password);
					this.Disconnect();
				}
				else
				{
					this.Error("Unable to get Auth Key \n" + ex, ErrorType.AuthKey);
					this.Disconnect();
				}
				result = false;
			}
			return result;
		}

		private int HexToInt(string hex)
		{
			int num = 0;
			for (int i = 0; i < hex.Length; i++)
			{
				char c = hex.ToCharArray()[i];
				if (c >= '0' && c <= '9')
				{
					num = num * 16 + (int)c - 48;
				}
				else
				{
					num = num * 16 + (int)c - 97 + 10;
				}
			}
			return num;
		}

		private bool GetIpAddress()
		{
			bool result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				WebRequest webRequest = WebRequest.Create("http://ll.leagueoflegends.com/services/connection_info");
				WebResponse response = webRequest.GetResponse();
				int num;
				while ((num = response.GetResponseStream().ReadByte()) != -1)
				{
					stringBuilder.Append((char)num);
				}
				webRequest.Abort();
				TypedObject typedObject = this.serializer.Deserialize<TypedObject>(stringBuilder.ToString());
				this.ipAddress = typedObject.GetString("ip_address");
				result = true;
			}
			catch (Exception ex)
			{
				this.Error("Unable to connect to Riot Games web server \n" + ex.Message, ErrorType.General);
				this.Disconnect();
				result = false;
			}
			return result;
		}

		private bool Handshake()
		{
			byte[] array = new byte[1537];
			this.rand.NextBytes(array);
			array[0] = 3;
			this.sslStream.Write(array);
			byte b = (byte)this.sslStream.ReadByte();
			if (b != 3)
			{
				this.Error("Server returned incorrect version in handshake: " + b, ErrorType.Handshake);
				this.Disconnect();
				return false;
			}
			byte[] buffer = new byte[1536];
			this.sslStream.Read(buffer, 0, 1536);
			this.sslStream.Write(buffer);
			byte[] array2 = new byte[1536];
			this.sslStream.Read(array2, 0, 1536);
			bool flag = true;
			for (int i = 8; i < 1536; i++)
			{
				if (array[i + 1] != array2[i])
				{
					flag = false;
					break;
				}
			}
			if (!flag)
			{
				this.Error("Server returned invalid handshake", ErrorType.Handshake);
				this.Disconnect();
				return false;
			}
			return true;
		}

		private bool SendConnect()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("app", "");
			dictionary.Add("flashVer", "WIN 10,6,602,161");
			dictionary.Add("swfUrl", "app:/LolClient.swf/[[DYNAMIC]]/32");
			dictionary.Add("tcUrl", string.Concat(new object[]
			{
				"rtmps://",
				this.server,
				":",
				2099
			}));
			dictionary.Add("fpad", false);
			dictionary.Add("capabilities", 239);
			dictionary.Add("audioCodecs", 3575);
			dictionary.Add("videoCodecs", 252);
			dictionary.Add("videoFunction", 1);
			dictionary.Add("pageUrl", null);
			dictionary.Add("objectEncoding", 3);
			RTMPSEncoder rTMPSEncoder = new RTMPSEncoder();
			byte[] array = rTMPSEncoder.EncodeConnect(dictionary);
			this.sslStream.Write(array, 0, array.Length);
			while (!this.results.ContainsKey(1))
			{
				Thread.Sleep(10);
			}
			TypedObject typedObject = this.results[1];
			this.results.Remove(1);
			if (typedObject["result"].Equals("_error"))
			{
				this.Error(this.GetErrorMessage(typedObject), ErrorType.Connect);
				this.Disconnect();
				return false;
			}
			this.DSId = typedObject.GetTO("data").GetString("id");
			this.isConnected = true;
			if (this.OnConnect != null)
			{
				this.OnConnect(this, EventArgs.Empty);
			}
			return true;
		}

		private bool Login()
		{
			AuthenticationCredentials authenticationCredentials = new AuthenticationCredentials();
			authenticationCredentials.Password = this.password;
			authenticationCredentials.ClientVersion = this.clientVersion;
			authenticationCredentials.IpAddress = this.ipAddress;
			authenticationCredentials.SecurityAnswer = null;
			authenticationCredentials.Locale = this.locale;
			authenticationCredentials.Domain = "lolclient.lol.riotgames.com";
			authenticationCredentials.OldPassword = null;
			authenticationCredentials.AuthToken = this.authToken;
			if (this.useGarena)
			{
				authenticationCredentials.PartnerCredentials = "8393 " + this.garenaToken;
				authenticationCredentials.Username = this.userID;
			}
			else
			{
				authenticationCredentials.PartnerCredentials = null;
				authenticationCredentials.Username = this.user;
			}
			int id = this.Invoke("loginService", "login", new object[]
			{
				authenticationCredentials.GetBaseTypedObject()
			});
			TypedObject result = this.GetResult(id);
			if (result["result"].Equals("_error"))
			{
				string arg_104_0 = (string)result.GetTO("data").GetTO("rootCause").GetArray("substitutionArguments")[1];
				this.Error(this.GetErrorMessage(result), ErrorType.Login);
				this.Disconnect();
				return false;
			}
			TypedObject typedObject = result.GetTO("data").GetTO("body");
			this.sessionToken = typedObject.GetString("token");
			this.accountID = typedObject.GetTO("accountSummary").GetInt("accountId").Value;
			if (this.useGarena)
			{
				typedObject = this.WrapBody(Convert.ToBase64String(Encoding.UTF8.GetBytes(this.userID + ":" + this.sessionToken)), "auth", 8);
			}
			else
			{
				typedObject = this.WrapBody(Convert.ToBase64String(Encoding.UTF8.GetBytes(this.user.ToLower() + ":" + this.sessionToken)), "auth", 8);
			}
			typedObject.type = "flex.messaging.messages.CommandMessage";
			id = this.Invoke(typedObject);
			result = this.GetResult(id);
			this.isLoggedIn = true;
			if (this.OnLogin != null)
			{
				this.OnLogin(this, this.user, this.ipAddress);
			}
			return true;
		}

		private string GetErrorMessage(TypedObject message)
		{
			return message.GetTO("data").GetTO("rootCause").GetString("message");
		}

		private string GetErrorCode(TypedObject message)
		{
			return message.GetTO("data").GetTO("rootCause").GetString("errorCode");
		}

		private void StartHeartbeat()
		{
			this.heartbeatThread = new Thread(delegate
			{
				LoLConnection.<<StartHeartbeat>b__4>d__5 <<StartHeartbeat>b__4>d__;
				<<StartHeartbeat>b__4>d__.<>4__this = this;
				<<StartHeartbeat>b__4>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<StartHeartbeat>b__4>d__.<>1__state = -1;
				AsyncVoidMethodBuilder <>t__builder = <<StartHeartbeat>b__4>d__.<>t__builder;
				<>t__builder.Start<LoLConnection.<<StartHeartbeat>b__4>d__5>(ref <<StartHeartbeat>b__4>d__);
			});
			this.heartbeatThread.Start();
		}

		public void Disconnect()
		{
			Thread thread = new Thread(delegate
			{
				if (this.isConnected)
				{
					int id = this.Invoke("loginService", "logout", new object[]
					{
						this.authToken
					});
					this.Join(id);
				}
				this.isConnected = false;
				if (this.heartbeatThread != null)
				{
					this.heartbeatThread.Abort();
				}
				if (this.decodeThread != null)
				{
					this.decodeThread.Abort();
				}
				this.invokeID = 2;
				this.heartbeatCount = 1;
				this.pendingInvokes.Clear();
				this.callbacks.Clear();
				this.results.Clear();
				this.client = null;
				this.sslStream = null;
				if (this.OnDisconnect != null)
				{
					this.OnDisconnect(this, EventArgs.Empty);
				}
			});
			thread.Start();
		}

		private void Error(string message, string errorCode, ErrorType type)
		{
			Error error = new Error
			{
				Type = type,
				Message = message,
				ErrorCode = errorCode
			};
			if (this.OnError != null)
			{
				this.OnError(this, error);
			}
		}

		private void Error(string message, ErrorType type)
		{
			this.Error(message, "", type);
		}

		private int Invoke(TypedObject packet)
		{
			int result;
			lock (this.isInvokingLock)
			{
				int num = this.NextInvokeID();
				this.pendingInvokes.Add(num);
				try
				{
					RTMPSEncoder rTMPSEncoder = new RTMPSEncoder();
					byte[] array = rTMPSEncoder.EncodeInvoke(num, packet);
					this.sslStream.Write(array, 0, array.Length);
					result = num;
				}
				catch (IOException ex)
				{
					this.pendingInvokes.Remove(num);
					throw ex;
				}
			}
			return result;
		}

		private int Invoke(string destination, object operation, object body)
		{
			return this.Invoke(this.WrapBody(body, destination, operation));
		}

		private int InvokeWithCallback(string destination, object operation, object body, RiotGamesObject cb)
		{
			if (this.isConnected)
			{
				this.callbacks.Add(this.invokeID, cb);
				return this.Invoke(destination, operation, body);
			}
			this.Error("The client is not connected. Please make sure to connect before tring to execute an Invoke command.", ErrorType.Invoke);
			this.Disconnect();
			return -1;
		}

		protected TypedObject WrapBody(object body, string destination, object operation)
		{
			TypedObject typedObject = new TypedObject();
			typedObject.Add("DSRequestTimeout", 60);
			typedObject.Add("DSId", this.DSId);
			typedObject.Add("DSEndpoint", "my-rtmps");
			return new TypedObject("flex.messaging.messages.RemotingMessage")
			{
				{
					"operation",
					operation
				},
				{
					"source",
					null
				},
				{
					"timestamp",
					0
				},
				{
					"messageId",
					RTMPSEncoder.RandomUID()
				},
				{
					"timeToLive",
					0
				},
				{
					"clientId",
					null
				},
				{
					"destination",
					destination
				},
				{
					"body",
					body
				},
				{
					"headers",
					typedObject
				}
			};
		}

		protected int NextInvokeID()
		{
			return this.invokeID++;
		}

		private void MessageReceived(object messageBody)
		{
			if (this.OnMessageReceived != null)
			{
				this.OnMessageReceived(this, messageBody);
			}
		}

		private void BeginReceive()
		{
			this.decodeThread = new Thread(delegate
			{
				try
				{
					Dictionary<int, Packet> dictionary = new Dictionary<int, Packet>();
					Dictionary<int, Packet> dictionary2 = new Dictionary<int, Packet>();
					while (true)
					{
						byte b = (byte)this.sslStream.ReadByte();
						List<byte> list = new List<byte>();
						if (b == 255)
						{
							this.Disconnect();
						}
						int key = 0;
						if ((b & 3) != 0)
						{
							key = (int)(b & 63);
							list.Add(b);
						}
						else if ((b & 1) != 0)
						{
							byte b2 = (byte)this.sslStream.ReadByte();
							key = (int)(64 + b2);
							list.Add(b);
							list.Add(b2);
						}
						else if ((b & 2) != 0)
						{
							byte b3 = (byte)this.sslStream.ReadByte();
							byte b4 = (byte)this.sslStream.ReadByte();
							list.Add(b);
							list.Add(b3);
							list.Add(b4);
							key = (int)(64 + b3) + 256 * (int)b4;
						}
						int num = (int)(b & 192);
						int num2 = 0;
						if (num == 0)
						{
							num2 = 12;
						}
						else if (num == 64)
						{
							num2 = 8;
						}
						else if (num == 128)
						{
							num2 = 4;
						}
						else if (num == 192)
						{
							num2 = 0;
						}
						if (!dictionary2.ContainsKey(key))
						{
							dictionary2.Add(key, new Packet());
						}
						Packet packet = dictionary2[key];
						packet.AddToRaw(list.ToArray());
						if (num2 == 12)
						{
							byte[] array = new byte[3];
							for (int i = 0; i < 3; i++)
							{
								array[i] = (byte)this.sslStream.ReadByte();
								packet.AddToRaw(array[i]);
							}
							byte[] array2 = new byte[3];
							for (int j = 0; j < 3; j++)
							{
								array2[j] = (byte)this.sslStream.ReadByte();
								packet.AddToRaw(array2[j]);
							}
							int num3 = 0;
							for (int k = 0; k < 3; k++)
							{
								num3 = num3 * 256 + (int)(array2[k] & 255);
							}
							packet.SetSize(num3);
							int num4 = this.sslStream.ReadByte();
							packet.AddToRaw((byte)num4);
							packet.SetType(num4);
							byte[] array3 = new byte[4];
							for (int l = 0; l < 4; l++)
							{
								array3[l] = (byte)this.sslStream.ReadByte();
								packet.AddToRaw(array3[l]);
							}
						}
						else if (num2 == 8)
						{
							byte[] array4 = new byte[3];
							for (int m = 0; m < 3; m++)
							{
								array4[m] = (byte)this.sslStream.ReadByte();
								packet.AddToRaw(array4[m]);
							}
							byte[] array5 = new byte[3];
							for (int n = 0; n < 3; n++)
							{
								array5[n] = (byte)this.sslStream.ReadByte();
								packet.AddToRaw(array5[n]);
							}
							int num5 = 0;
							for (int num6 = 0; num6 < 3; num6++)
							{
								num5 = num5 * 256 + (int)(array5[num6] & 255);
							}
							packet.SetSize(num5);
							int num7 = this.sslStream.ReadByte();
							packet.AddToRaw((byte)num7);
							packet.SetType(num7);
						}
						else if (num2 == 4)
						{
							byte[] array6 = new byte[3];
							for (int num8 = 0; num8 < 3; num8++)
							{
								array6[num8] = (byte)this.sslStream.ReadByte();
								packet.AddToRaw(array6[num8]);
							}
							if (packet.GetSize() == 0 && packet.GetPacketType() == 0 && dictionary.ContainsKey(key))
							{
								packet.SetSize(dictionary[key].GetSize());
								packet.SetType(dictionary[key].GetPacketType());
							}
						}
						else if (num2 == 0 && packet.GetSize() == 0 && packet.GetPacketType() == 0 && dictionary.ContainsKey(key))
						{
							packet.SetSize(dictionary[key].GetSize());
							packet.SetType(dictionary[key].GetPacketType());
						}
						for (int num9 = 0; num9 < 128; num9++)
						{
							byte b5 = (byte)this.sslStream.ReadByte();
							packet.Add(b5);
							packet.AddToRaw(b5);
							if (packet.IsComplete())
							{
								break;
							}
						}
						if (packet.IsComplete())
						{
							if (dictionary.ContainsKey(key))
							{
								dictionary.Remove(key);
							}
							dictionary.Add(key, packet);
							if (dictionary2.ContainsKey(key))
							{
								dictionary2.Remove(key);
							}
							RTMPSDecoder rTMPSDecoder = new RTMPSDecoder();
							TypedObject typedObject;
							if (packet.GetPacketType() == 20)
							{
								typedObject = rTMPSDecoder.DecodeConnect(packet.GetData());
							}
							else if (packet.GetPacketType() == 17)
							{
								typedObject = rTMPSDecoder.DecodeInvoke(packet.GetData());
							}
							else
							{
								if (packet.GetPacketType() == 6)
								{
									byte[] data = packet.GetData();
									int num10 = 0;
									for (int num11 = 0; num11 < 4; num11++)
									{
										num10 = num10 * 256 + (int)(data[num11] & 255);
									}
									byte arg_4B3_0 = data[4];
									continue;
								}
								if (packet.GetPacketType() == 5)
								{
									byte[] data2 = packet.GetData();
									int num12 = 0;
									for (int num13 = 0; num13 < 4; num13++)
									{
										num12 = num12 * 256 + (int)(data2[num13] & 255);
									}
									continue;
								}
								if (packet.GetPacketType() == 3)
								{
									byte[] data3 = packet.GetData();
									int num14 = 0;
									for (int num15 = 0; num15 < 4; num15++)
									{
										num14 = num14 * 256 + (int)(data3[num15] & 255);
									}
									continue;
								}
								if (packet.GetPacketType() == 2)
								{
									packet.GetData();
									continue;
								}
								if (packet.GetPacketType() == 1)
								{
									packet.GetData();
									continue;
								}
								continue;
							}
							int? @int = typedObject.GetInt("invokeId");
							if (typedObject["result"].Equals("_error"))
							{
								this.Error(this.GetErrorMessage(typedObject), this.GetErrorCode(typedObject), ErrorType.Receive);
							}
							if (typedObject["result"].Equals("receive") && typedObject.GetTO("data") != null)
							{
								TypedObject to = typedObject.GetTO("data");
								if (to.ContainsKey("body") && to["body"] is TypedObject)
								{
									new Thread(delegate
									{
										TypedObject typedObject2 = (TypedObject)to["body"];
										if (typedObject2.type.Equals("com.riotgames.platform.game.GameDTO"))
										{
											this.MessageReceived(new GameDTO(typedObject2));
											return;
										}
										if (typedObject2.type.Equals("com.riotgames.platform.game.PlayerCredentialsDto"))
										{
											this.MessageReceived(new PlayerCredentialsDto(typedObject2));
											return;
										}
										if (typedObject2.type.Equals("com.riotgames.platform.game.message.GameNotification"))
										{
											this.MessageReceived(new GameNotification(typedObject2));
											return;
										}
										if (typedObject2.type.Equals("com.riotgames.platform.matchmaking.SearchingForMatchNotification"))
										{
											this.MessageReceived(new SearchingForMatchNotification(typedObject2));
											return;
										}
										if (typedObject2.type.Equals("com.riotgames.platform.messaging.StoreFulfillmentNotification"))
										{
											this.MessageReceived(new StoreFulfillmentNotification(typedObject2));
											return;
										}
										if (typedObject2.type.Equals("com.riotgames.platform.messaging.StoreFulfillmentNotification"))
										{
											this.MessageReceived(new StoreAccountBalanceNotification(typedObject2));
											return;
										}
										this.MessageReceived(typedObject2);
									}).Start();
								}
							}
							if (@int.HasValue)
							{
								if (!(@int == 0))
								{
									if (this.callbacks.ContainsKey(@int.Value))
									{
										RiotGamesObject cb = this.callbacks[@int.Value];
										this.callbacks.Remove(@int.Value);
										if (cb != null)
										{
											TypedObject messageBody = typedObject.GetTO("data").GetTO("body");
											new Thread(delegate
											{
												cb.DoCallback(messageBody);
											}).Start();
										}
									}
									else
									{
										this.results.Add(@int.Value, typedObject);
									}
								}
								this.pendingInvokes.Remove(@int.Value);
							}
						}
					}
				}
				catch (Exception ex)
				{
					if (this.IsConnected())
					{
						this.Error(ex.Message, ErrorType.Receive);
					}
				}
			});
			this.decodeThread.Start();
		}

		private TypedObject GetResult(int id)
		{
			while (this.IsConnected() && !this.results.ContainsKey(id))
			{
				Thread.Sleep(10);
			}
			if (!this.IsConnected())
			{
				return null;
			}
			TypedObject result = this.results[id];
			this.results.Remove(id);
			return result;
		}

		private TypedObject PeekResult(int id)
		{
			if (this.results.ContainsKey(id))
			{
				TypedObject result = this.results[id];
				this.results.Remove(id);
				return result;
			}
			return null;
		}

		private void Join()
		{
			while (this.pendingInvokes.Count > 0)
			{
				Thread.Sleep(10);
			}
		}

		private void Join(int id)
		{
			while (this.IsConnected() && this.pendingInvokes.Contains(id))
			{
				Thread.Sleep(10);
			}
		}

		private void Cancel(int id)
		{
			this.pendingInvokes.Remove(id);
			if (this.PeekResult(id) != null)
			{
				return;
			}
			this.callbacks.Add(id, null);
			if (this.PeekResult(id) != null)
			{
				this.callbacks.Remove(id);
			}
		}

		public bool IsConnected()
		{
			return this.isConnected;
		}

		public bool IsLoggedIn()
		{
			return this.isLoggedIn;
		}

		public double AccountID()
		{
			return (double)this.accountID;
		}

		private void Login(AuthenticationCredentials arg0, Session.Callback callback)
		{
			Session cb = new Session(callback);
			this.InvokeWithCallback("loginService", "login", new object[]
			{
				arg0.GetBaseTypedObject()
			}, cb);
		}

		private async Task<Session> Login(AuthenticationCredentials arg0)
		{
			int key = this.Invoke("loginService", "login", new object[]
			{
				arg0.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			Session result = new Session(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> Subscribe(string service, double accountId)
		{
			TypedObject typedObject = this.WrapBody(new TypedObject(), "messagingDestination", 0);
			typedObject.type = "flex.messaging.messages.CommandMessage";
			TypedObject tO = typedObject.GetTO("headers");
			if (service == "bc")
			{
				tO.Add("DSSubtopic", "bc");
			}
			else
			{
				tO.Add("DSSubtopic", service + "-" + this.accountID);
			}
			tO.Remove("DSRequestTimeout");
			typedObject["clientId"] = service + "-" + this.accountID;
			int num = this.Invoke(typedObject);
			while (!this.results.ContainsKey(num))
			{
				await Task.Delay(10);
			}
			this.GetResult(num);
			return null;
		}

		public void GetLoginDataPacketForUser(LoginDataPacket.Callback callback)
		{
			LoginDataPacket cb = new LoginDataPacket(callback);
			this.InvokeWithCallback("clientFacadeService", "getLoginDataPacketForUser", new object[0], cb);
		}

		public async Task<LoginDataPacket> GetLoginDataPacketForUser()
		{
			int key = this.Invoke("clientFacadeService", "getLoginDataPacketForUser", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			LoginDataPacket result = new LoginDataPacket(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<GameQueueConfig[]> GetAvailableQueues()
		{
			int key = this.Invoke("matchmakerService", "getAvailableQueues", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			GameQueueConfig[] array = new GameQueueConfig[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = new GameQueueConfig((TypedObject)this.results[key].GetTO("data").GetArray("body")[i]);
			}
			this.results.Remove(key);
			return array;
		}

		public void GetSumonerActiveBoosts(SummonerActiveBoostsDTO.Callback callback)
		{
			SummonerActiveBoostsDTO cb = new SummonerActiveBoostsDTO(callback);
			this.InvokeWithCallback("inventoryService", "getSumonerActiveBoosts", new object[0], cb);
		}

		public async Task<SummonerActiveBoostsDTO> GetSumonerActiveBoosts()
		{
			int key = this.Invoke("inventoryService", "getSumonerActiveBoosts", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SummonerActiveBoostsDTO result = new SummonerActiveBoostsDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<ChampionDTO[]> GetAvailableChampions()
		{
			int key = this.Invoke("inventoryService", "getAvailableChampions", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			ChampionDTO[] array = new ChampionDTO[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = new ChampionDTO((TypedObject)this.results[key].GetTO("data").GetArray("body")[i]);
			}
			this.results.Remove(key);
			return array;
		}

		public void GetSummonerRuneInventory(double summonerId, SummonerRuneInventory.Callback callback)
		{
			SummonerRuneInventory cb = new SummonerRuneInventory(callback);
			this.InvokeWithCallback("summonerRuneService", "getSummonerRuneInventory", new object[]
			{
				summonerId
			}, cb);
		}

		public async Task<SummonerRuneInventory> GetSummonerRuneInventory(double summonerId)
		{
			int key = this.Invoke("summonerRuneService", "getSummonerRuneInventory", new object[]
			{
				summonerId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SummonerRuneInventory result = new SummonerRuneInventory(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<string> PerformLCDSHeartBeat(int arg0, string arg1, int arg2, string arg3)
		{
			int key = this.Invoke("loginService", "performLCDSHeartBeat", new object[]
			{
				arg0,
				arg1,
				arg2,
				arg3
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			string result = (string)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public void GetMyLeaguePositions(SummonerLeagueItemsDTO.Callback callback)
		{
			SummonerLeagueItemsDTO cb = new SummonerLeagueItemsDTO(callback);
			this.InvokeWithCallback("leaguesServiceProxy", "getMyLeaguePositions", new object[0], cb);
		}

		public async Task<SummonerLeagueItemsDTO> GetMyLeaguePositions()
		{
			int key = this.Invoke("leaguesServiceProxy", "getMyLeaguePositions", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SummonerLeagueItemsDTO result = new SummonerLeagueItemsDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> LoadPreferencesByKey(string arg0, double arg1, bool arg2)
		{
			int key = this.Invoke("playerPreferencesService", "loadPreferencesByKey", new object[]
			{
				arg0,
				arg1,
				arg2
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public void GetMasteryBook(double summonerId, MasteryBookDTO.Callback callback)
		{
			MasteryBookDTO cb = new MasteryBookDTO(callback);
			this.InvokeWithCallback("masteryBookService", "getMasteryBook", new object[]
			{
				summonerId
			}, cb);
		}

		public async Task<MasteryBookDTO> GetMasteryBook(double summonerId)
		{
			int key = this.Invoke("masteryBookService", "getMasteryBook", new object[]
			{
				summonerId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			MasteryBookDTO result = new MasteryBookDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void CreatePlayer(PlayerDTO.Callback callback)
		{
			PlayerDTO cb = new PlayerDTO(callback);
			this.InvokeWithCallback("summonerTeamService", "createPlayer", new object[0], cb);
		}

		public async Task<PlayerDTO> CreatePlayer()
		{
			int key = this.Invoke("summonerTeamService", "createPlayer", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			PlayerDTO result = new PlayerDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<AllSummonerData> CreateDefaultSummoner(string summonerName)
		{
			int key = this.Invoke("summonerService", "createDefaultSummoner", new object[]
			{
				summonerName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			AllSummonerData result = new AllSummonerData(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<string[]> GetSummonerNames(double[] summonerIds)
		{
			int key = this.Invoke("summonerService", "getSummonerNames", new object[]
			{
				summonerIds.Cast<object>().ToArray<object>()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			string[] array = new string[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = (string)this.results[key].GetTO("data").GetArray("body")[i];
			}
			this.results.Remove(key);
			return array;
		}

		public void GetChallengerLeague(string queueType, LeagueListDTO.Callback callback)
		{
			LeagueListDTO cb = new LeagueListDTO(callback);
			this.InvokeWithCallback("leaguesServiceProxy", "getChallengerLeague", new object[]
			{
				queueType
			}, cb);
		}

		public async Task<LeagueListDTO> GetChallengerLeague(string queueType)
		{
			int key = this.Invoke("leaguesServiceProxy", "getChallengerLeague", new object[]
			{
				queueType
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			LeagueListDTO result = new LeagueListDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetAllMyLeagues(SummonerLeaguesDTO.Callback callback)
		{
			SummonerLeaguesDTO cb = new SummonerLeaguesDTO(callback);
			this.InvokeWithCallback("leaguesServiceProxy", "getAllMyLeagues", new object[0], cb);
		}

		public async Task<SummonerLeaguesDTO> GetAllMyLeagues()
		{
			int key = this.Invoke("leaguesServiceProxy", "getAllMyLeagues", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SummonerLeaguesDTO result = new SummonerLeaguesDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetAllSummonerDataByAccount(double accountId, AllSummonerData.Callback callback)
		{
			AllSummonerData cb = new AllSummonerData(callback);
			this.InvokeWithCallback("summonerService", "getAllSummonerDataByAccount", new object[]
			{
				accountId
			}, cb);
		}

		public async Task<AllSummonerData> GetAllSummonerDataByAccount(double accountId)
		{
			int key = this.Invoke("summonerService", "getAllSummonerDataByAccount", new object[]
			{
				accountId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			AllSummonerData result = new AllSummonerData(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetPointsBalance(PointSummary.Callback callback)
		{
			PointSummary cb = new PointSummary(callback);
			this.InvokeWithCallback("lcdsRerollService", "getPointsBalance", new object[0], cb);
		}

		public async Task<PointSummary> GetPointsBalance()
		{
			int key = this.Invoke("lcdsRerollService", "getPointsBalance", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			PointSummary result = new PointSummary(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<string> GetSummonerIcons(double[] summonerIds)
		{
			int key = this.Invoke("summonerService", "getSummonerIcons", new object[]
			{
				summonerIds.Cast<object>().ToArray<object>()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			string result = (string)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public void CallKudos(string arg0, LcdsResponseString.Callback callback)
		{
			LcdsResponseString cb = new LcdsResponseString(callback);
			this.InvokeWithCallback("clientFacadeService", "callKudos", new object[]
			{
				arg0
			}, cb);
		}

		public async Task<LcdsResponseString> CallKudos(string arg0)
		{
			int key = this.Invoke("clientFacadeService", "callKudos", new object[]
			{
				arg0
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			LcdsResponseString result = new LcdsResponseString(tO);
			this.results.Remove(key);
			return result;
		}

		public void RetrievePlayerStatsByAccountId(double accountId, string season, PlayerLifetimeStats.Callback callback)
		{
			PlayerLifetimeStats cb = new PlayerLifetimeStats(callback);
			this.InvokeWithCallback("playerStatsService", "retrievePlayerStatsByAccountId", new object[]
			{
				accountId,
				season
			}, cb);
		}

		public async Task<PlayerLifetimeStats> RetrievePlayerStatsByAccountId(double accountId, string season)
		{
			int key = this.Invoke("playerStatsService", "retrievePlayerStatsByAccountId", new object[]
			{
				accountId,
				season
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			PlayerLifetimeStats result = new PlayerLifetimeStats(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<ChampionStatInfo[]> RetrieveTopPlayedChampions(double accountId, string gameMode)
		{
			int key = this.Invoke("playerStatsService", "retrieveTopPlayedChampions", new object[]
			{
				accountId,
				gameMode
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			ChampionStatInfo[] array = new ChampionStatInfo[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = new ChampionStatInfo((TypedObject)this.results[key].GetTO("data").GetArray("body")[i]);
			}
			this.results.Remove(key);
			return array;
		}

		public void GetSummonerByName(string summonerName, PublicSummoner.Callback callback)
		{
			PublicSummoner cb = new PublicSummoner(callback);
			this.InvokeWithCallback("summonerService", "getSummonerByName", new object[]
			{
				summonerName
			}, cb);
		}

		public async Task<PublicSummoner> GetSummonerByName(string summonerName)
		{
			int key = this.Invoke("summonerService", "getSummonerByName", new object[]
			{
				summonerName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			PublicSummoner result = new PublicSummoner(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetAggregatedStats(double summonerId, string gameMode, string season, AggregatedStats.Callback callback)
		{
			AggregatedStats cb = new AggregatedStats(callback);
			this.InvokeWithCallback("playerStatsService", "getAggregatedStats", new object[]
			{
				summonerId,
				gameMode,
				season
			}, cb);
		}

		public async Task<AggregatedStats> GetAggregatedStats(double summonerId, string gameMode, string season)
		{
			int key = this.Invoke("playerStatsService", "getAggregatedStats", new object[]
			{
				summonerId,
				gameMode,
				season
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			AggregatedStats result = new AggregatedStats(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetRecentGames(double accountId, RecentGames.Callback callback)
		{
			RecentGames cb = new RecentGames(callback);
			this.InvokeWithCallback("playerStatsService", "getRecentGames", new object[]
			{
				accountId
			}, cb);
		}

		public async Task<RecentGames> GetRecentGames(double accountId)
		{
			int key = this.Invoke("playerStatsService", "getRecentGames", new object[]
			{
				accountId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			RecentGames result = new RecentGames(tO);
			this.results.Remove(key);
			return result;
		}

		public void FindTeamById(TeamId teamId, TeamDTO.Callback callback)
		{
			TeamDTO cb = new TeamDTO(callback);
			this.InvokeWithCallback("summonerTeamService", "findTeamById", new object[]
			{
				teamId.GetBaseTypedObject()
			}, cb);
		}

		public async Task<TeamDTO> FindTeamById(TeamId teamId)
		{
			int key = this.Invoke("summonerTeamService", "findTeamById", new object[]
			{
				teamId.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			TeamDTO result = new TeamDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetLeaguesForTeam(string teamName, SummonerLeaguesDTO.Callback callback)
		{
			SummonerLeaguesDTO cb = new SummonerLeaguesDTO(callback);
			this.InvokeWithCallback("leaguesServiceProxy", "getLeaguesForTeam", new object[]
			{
				teamName
			}, cb);
		}

		public async Task<SummonerLeaguesDTO> GetLeaguesForTeam(string teamName)
		{
			int key = this.Invoke("leaguesServiceProxy", "getLeaguesForTeam", new object[]
			{
				teamName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SummonerLeaguesDTO result = new SummonerLeaguesDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<TeamAggregatedStatsDTO[]> GetTeamAggregatedStats(TeamId arg0)
		{
			int key = this.Invoke("playerStatsService", "getTeamAggregatedStats", new object[]
			{
				arg0.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TeamAggregatedStatsDTO[] array = new TeamAggregatedStatsDTO[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = new TeamAggregatedStatsDTO((TypedObject)this.results[key].GetTO("data").GetArray("body")[i]);
			}
			this.results.Remove(key);
			return array;
		}

		public void GetTeamEndOfGameStats(TeamId arg0, double arg1, EndOfGameStats.Callback callback)
		{
			EndOfGameStats cb = new EndOfGameStats(callback);
			this.InvokeWithCallback("playerStatsService", "getTeamEndOfGameStats", new object[]
			{
				arg0.GetBaseTypedObject(),
				arg1
			}, cb);
		}

		public async Task<EndOfGameStats> GetTeamEndOfGameStats(TeamId arg0, double arg1)
		{
			int key = this.Invoke("playerStatsService", "getTeamEndOfGameStats", new object[]
			{
				arg0.GetBaseTypedObject(),
				arg1
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			EndOfGameStats result = new EndOfGameStats(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> DisbandTeam(TeamId teamId)
		{
			int key = this.Invoke("summonerTeamService", "disbandTeam", new object[]
			{
				teamId.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<bool> IsNameValidAndAvailable(string teamName)
		{
			int key = this.Invoke("summonerTeamService", "isNameValidAndAvailable", new object[]
			{
				teamName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<bool> IsTagValidAndAvailable(string tagName)
		{
			int key = this.Invoke("summonerTeamService", "isTagValidAndAvailable", new object[]
			{
				tagName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public void CreateTeam(string teamName, string tagName, TeamDTO.Callback callback)
		{
			TeamDTO cb = new TeamDTO(callback);
			this.InvokeWithCallback("summonerTeamService", "createTeam", new object[]
			{
				teamName,
				tagName
			}, cb);
		}

		public async Task<TeamDTO> CreateTeam(string teamName, string tagName)
		{
			int key = this.Invoke("summonerTeamService", "createTeam", new object[]
			{
				teamName,
				tagName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			TeamDTO result = new TeamDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void InvitePlayer(double summonerId, TeamId teamId, TeamDTO.Callback callback)
		{
			TeamDTO cb = new TeamDTO(callback);
			this.InvokeWithCallback("summonerTeamService", "invitePlayer", new object[]
			{
				summonerId,
				teamId.GetBaseTypedObject()
			}, cb);
		}

		public async Task<TeamDTO> InvitePlayer(double summonerId, TeamId teamId)
		{
			int key = this.Invoke("summonerTeamService", "invitePlayer", new object[]
			{
				summonerId,
				teamId.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			TeamDTO result = new TeamDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void KickPlayer(double summonerId, TeamId teamId, TeamDTO.Callback callback)
		{
			TeamDTO cb = new TeamDTO(callback);
			this.InvokeWithCallback("summonerTeamService", "kickPlayer", new object[]
			{
				summonerId,
				teamId.GetBaseTypedObject()
			}, cb);
		}

		public async Task<TeamDTO> KickPlayer(double summonerId, TeamId teamId)
		{
			int key = this.Invoke("summonerTeamService", "kickPlayer", new object[]
			{
				summonerId,
				teamId.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			TeamDTO result = new TeamDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetAllLeaguesForPlayer(double summonerId, SummonerLeaguesDTO.Callback callback)
		{
			SummonerLeaguesDTO cb = new SummonerLeaguesDTO(callback);
			this.InvokeWithCallback("leaguesServiceProxy", "getAllLeaguesForPlayer", new object[]
			{
				summonerId
			}, cb);
		}

		public async Task<SummonerLeaguesDTO> GetAllLeaguesForPlayer(double summonerId)
		{
			int key = this.Invoke("leaguesServiceProxy", "getAllLeaguesForPlayer", new object[]
			{
				summonerId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SummonerLeaguesDTO result = new SummonerLeaguesDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetAllPublicSummonerDataByAccount(double accountId, AllPublicSummonerDataDTO.Callback callback)
		{
			AllPublicSummonerDataDTO cb = new AllPublicSummonerDataDTO(callback);
			this.InvokeWithCallback("summonerService", "getAllPublicSummonerDataByAccount", new object[]
			{
				accountId
			}, cb);
		}

		public async Task<AllPublicSummonerDataDTO> GetAllPublicSummonerDataByAccount(double accountId)
		{
			int key = this.Invoke("summonerService", "getAllPublicSummonerDataByAccount", new object[]
			{
				accountId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			AllPublicSummonerDataDTO result = new AllPublicSummonerDataDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void FindPlayer(double summonerId, PlayerDTO.Callback callback)
		{
			PlayerDTO cb = new PlayerDTO(callback);
			this.InvokeWithCallback("summonerTeamService", "findPlayer", new object[]
			{
				summonerId
			}, cb);
		}

		public async Task<PlayerDTO> FindPlayer(double summonerId)
		{
			int key = this.Invoke("summonerTeamService", "findPlayer", new object[]
			{
				summonerId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			PlayerDTO result = new PlayerDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void GetSpellBook(double summonerId, SpellBookDTO.Callback callback)
		{
			SpellBookDTO cb = new SpellBookDTO(callback);
			this.InvokeWithCallback("spellBookService", "getSpellBook", new object[]
			{
				summonerId
			}, cb);
		}

		public async Task<SpellBookDTO> GetSpellBook(double summonerId)
		{
			int key = this.Invoke("spellBookService", "getSpellBook", new object[]
			{
				summonerId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SpellBookDTO result = new SpellBookDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public void AttachToQueue(MatchMakerParams matchMakerParams, SearchingForMatchNotification.Callback callback)
		{
			SearchingForMatchNotification cb = new SearchingForMatchNotification(callback);
			this.InvokeWithCallback("matchmakerService", "attachToQueue", new object[]
			{
				matchMakerParams.GetBaseTypedObject()
			}, cb);
		}

		public async Task<SearchingForMatchNotification> AttachToQueue(MatchMakerParams matchMakerParams)
		{
			int key = this.Invoke("matchmakerService", "attachToQueue", new object[]
			{
				matchMakerParams.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SearchingForMatchNotification result = new SearchingForMatchNotification(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<bool> CancelFromQueueIfPossible(int queueId)
		{
			int key = this.Invoke("matchmakerService", "cancelFromQueueIfPossible", new object[]
			{
				queueId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<string> GetStoreUrl()
		{
			int key = this.Invoke("loginService", "getStoreUrl", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			string result = (string)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<PracticeGameSearchResult[]> ListAllPracticeGames()
		{
			int key = this.Invoke("gameService", "listAllPracticeGames", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			PracticeGameSearchResult[] array = new PracticeGameSearchResult[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = new PracticeGameSearchResult((TypedObject)this.results[key].GetTO("data").GetArray("body")[i]);
			}
			this.results.Remove(key);
			return array;
		}

		public async Task<object> JoinGame(double gameId)
		{
			string arg_37_1 = "gameService";
			object arg_37_2 = "joinGame";
			object[] array = new object[2];
			array[0] = gameId;
			int key = this.Invoke(arg_37_1, arg_37_2, array);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> JoinGame(double gameId, string password)
		{
			int key = this.Invoke("gameService", "joinGame", new object[]
			{
				gameId,
				password
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> ObserveGame(double gameId)
		{
			string arg_37_1 = "gameService";
			object arg_37_2 = "observeGame";
			object[] array = new object[2];
			array[0] = gameId;
			int key = this.Invoke(arg_37_1, arg_37_2, array);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> ObserveGame(double gameId, string password)
		{
			int key = this.Invoke("gameService", "observeGame", new object[]
			{
				gameId,
				password
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<string> GetSummonerInternalNameByName(string summonerName)
		{
			int key = this.Invoke("summonerService", "getSummonerInternalNameByName", new object[]
			{
				summonerName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			string result = (string)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<bool> SwitchTeams(double gameId)
		{
			int key = this.Invoke("gameService", "switchTeams", new object[]
			{
				gameId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<bool> SwitchPlayerToObserver(double gameId)
		{
			int key = this.Invoke("gameService", "switchPlayerToObserver", new object[]
			{
				gameId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<bool> SwitchObserverToPlayer(double gameId, int team)
		{
			int key = this.Invoke("gameService", "switchObserverToPlayer", new object[]
			{
				gameId,
				team
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<object> QuitGame()
		{
			int key = this.Invoke("gameService", "quitGame", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public void CreatePracticeGame(PracticeGameConfig practiceGameConfig, GameDTO.Callback callback)
		{
			GameDTO cb = new GameDTO(callback);
			this.InvokeWithCallback("gameService", "createPracticeGame", new object[]
			{
				practiceGameConfig.GetBaseTypedObject()
			}, cb);
		}

		public async Task<GameDTO> CreatePracticeGame(PracticeGameConfig practiceGameConfig)
		{
			int key = this.Invoke("gameService", "createPracticeGame", new object[]
			{
				practiceGameConfig.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			GameDTO result = new GameDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> SelectBotChampion(int arg0, BotParticipant arg1)
		{
			int key = this.Invoke("gameService", "selectBotChampion", new object[]
			{
				arg0,
				arg1.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> RemoveBotChampion(int arg0, BotParticipant arg1)
		{
			int key = this.Invoke("gameService", "removeBotChampion", new object[]
			{
				arg0,
				arg1.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public void StartChampionSelection(double gameId, double optomisticLock, StartChampSelectDTO.Callback callback)
		{
			StartChampSelectDTO cb = new StartChampSelectDTO(callback);
			this.InvokeWithCallback("gameService", "startChampionSelection", new object[]
			{
				gameId,
				optomisticLock
			}, cb);
		}

		public async Task<StartChampSelectDTO> StartChampionSelection(double gameId, double optomisticLock)
		{
			int key = this.Invoke("gameService", "startChampionSelection", new object[]
			{
				gameId,
				optomisticLock
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			StartChampSelectDTO result = new StartChampSelectDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> SetClientReceivedGameMessage(double gameId, string arg1)
		{
			int key = this.Invoke("gameService", "setClientReceivedGameMessage", new object[]
			{
				gameId,
				arg1
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public void GetLatestGameTimerState(double arg0, string arg1, int arg2, GameDTO.Callback callback)
		{
			GameDTO cb = new GameDTO(callback);
			this.InvokeWithCallback("gameService", "getLatestGameTimerState", new object[]
			{
				arg0,
				arg1,
				arg2
			}, cb);
		}

		public async Task<GameDTO> GetLatestGameTimerState(double arg0, string arg1, int arg2)
		{
			int key = this.Invoke("gameService", "getLatestGameTimerState", new object[]
			{
				arg0,
				arg1,
				arg2
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			GameDTO result = new GameDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> SelectSpells(int spellOneId, int spellTwoId)
		{
			int key = this.Invoke("gameService", "selectSpells", new object[]
			{
				spellOneId,
				spellTwoId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public void SelectDefaultSpellBookPage(SpellBookPageDTO spellBookPage, SpellBookPageDTO.Callback callback)
		{
			SpellBookPageDTO cb = new SpellBookPageDTO(callback);
			this.InvokeWithCallback("spellBookService", "selectDefaultSpellBookPage", new object[]
			{
				spellBookPage.GetBaseTypedObject()
			}, cb);
		}

		public async Task<SpellBookPageDTO> SelectDefaultSpellBookPage(SpellBookPageDTO spellBookPage)
		{
			int key = this.Invoke("spellBookService", "selectDefaultSpellBookPage", new object[]
			{
				spellBookPage.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SpellBookPageDTO result = new SpellBookPageDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> SelectChampion(int championId)
		{
			int key = this.Invoke("gameService", "selectChampion", new object[]
			{
				championId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> SelectChampionSkin(int championId, int skinId)
		{
			int key = this.Invoke("gameService", "selectChampionSkin", new object[]
			{
				championId,
				skinId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> ChampionSelectCompleted()
		{
			int key = this.Invoke("gameService", "championSelectCompleted", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> SetClientReceivedMaestroMessage(double arg0, string arg1)
		{
			int key = this.Invoke("gameService", "setClientReceivedMaestroMessage", new object[]
			{
				arg0,
				arg1
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public void RetrieveInProgressSpectatorGameInfo(string summonerName, PlatformGameLifecycleDTO.Callback callback)
		{
			PlatformGameLifecycleDTO cb = new PlatformGameLifecycleDTO(callback);
			this.InvokeWithCallback("gameService", "retrieveInProgressSpectatorGameInfo", new object[]
			{
				summonerName
			}, cb);
		}

		public async Task<PlatformGameLifecycleDTO> RetrieveInProgressSpectatorGameInfo(string summonerName)
		{
			int key = this.Invoke("gameService", "retrieveInProgressSpectatorGameInfo", new object[]
			{
				summonerName
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			PlatformGameLifecycleDTO result = new PlatformGameLifecycleDTO(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<bool> DeclineObserverReconnect()
		{
			int key = this.Invoke("gameService", "declineObserverReconnect", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			bool result = (bool)this.results[key].GetTO("data")["body"];
			this.results.Remove(key);
			return result;
		}

		public async Task<object> AcceptInviteForMatchmakingGame(double gameId)
		{
			int key = this.Invoke("matchmakerService", "acceptInviteForMatchmakingGame", new object[]
			{
				gameId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> AcceptPoppedGame(bool accept)
		{
			int key = this.Invoke("gameService", "acceptPoppedGame", new object[]
			{
				accept
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> UpdateProfileIconId(int iconId)
		{
			int key = this.Invoke("summonerService", "updateProfileIconId", new object[]
			{
				iconId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> BanUserFromGame(double gameId, double accountId)
		{
			int key = this.Invoke("gameService", "banUserFromGame", new object[]
			{
				gameId,
				accountId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<SearchingForMatchNotification> AttachToLowPriorityQueue(MatchMakerParams matchMakerParams, string accessToken)
		{
			TypedObject typedObject = new TypedObject(null);
			typedObject.Add("LEAVER_BUSTER_ACCESS_TOKEN", accessToken);
			int key = this.Invoke("matchmakerService", "attachToQueue", new object[]
			{
				matchMakerParams.GetBaseTypedObject(),
				typedObject
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			TypedObject tO = this.results[key].GetTO("data").GetTO("body");
			SearchingForMatchNotification result = new SearchingForMatchNotification(tO);
			this.results.Remove(key);
			return result;
		}

		public async Task<object> AcceptTrade(string SummonerInternalName, int ChampionId)
		{
			int key = this.Invoke("lcdsChampionTradeService", "attemptTrade", new object[]
			{
				SummonerInternalName,
				ChampionId,
				true
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> ackLeaverBusterWarning()
		{
			int key = this.Invoke("clientFacadeService", "ackLeaverBusterWarning", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> callPersistenceMessaging(SimpleDialogMessageResponse response)
		{
			int key = this.Invoke("clientFacadeService", "callPersistenceMessaging", new object[]
			{
				response.GetBaseTypedObject()
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> BanObserverFromGame(double gameId, double accountId)
		{
			int key = this.Invoke("gameService", "banObserverFromGame", new object[]
			{
				gameId,
				accountId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<object> BanChampion(int championId)
		{
			int key = this.Invoke("gameService", "banChampion", new object[]
			{
				championId
			});
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			this.results.Remove(key);
			return null;
		}

		public async Task<ChampionBanInfoDTO[]> GetChampionsForBan()
		{
			int key = this.Invoke("gameService", "getChampionsForBan", new object[0]);
			while (!this.results.ContainsKey(key))
			{
				await Task.Delay(10);
			}
			ChampionBanInfoDTO[] array = new ChampionBanInfoDTO[this.results[key].GetTO("data").GetArray("body").Length];
			for (int i = 0; i < this.results[key].GetTO("data").GetArray("body").Length; i++)
			{
				array[i] = new ChampionBanInfoDTO((TypedObject)this.results[key].GetTO("data").GetArray("body")[i]);
			}
			this.results.Remove(key);
			return array;
		}
	}
}
