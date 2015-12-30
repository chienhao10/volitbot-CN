using LoLLauncher;
using RitoBot;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VoliBot.Utils;

namespace VoliBot
{
	public class console : Form
	{
		public string currentVersion = "5.24.15_12_11_17_53";

		public string accounts_txt_path = "";

		public string lolPath = "";

		public int maxBots = 5;

		public int maxLevel = 10;

		public string region = "EUW";

		public string championToPick = "Annie";

		public string spell1 = "Heal";

		public string spell2 = "Flash";

		public bool randomSpell;

		public bool replaceConfig;

		public string queueType = "ARAM";

		public bool buyBoost;

		public ArrayList accounts = new ArrayList();

		public ArrayList accountsNew = new ArrayList();

		private IContainer components;

		private RichTextBox richTextBox1;

		public console(string cV, string atp, int mbots, int mlevel, string rgn, string ctp, string ss1, string ss2, bool rndss, bool rplccfg, string lolpath, string queuetyp)
		{
			this.currentVersion = cV;
			this.accounts_txt_path = atp;
			this.maxBots = mbots;
			this.maxLevel = mlevel;
			this.region = rgn;
			this.championToPick = ctp;
			this.spell1 = ss1;
			this.spell2 = ss2;
			this.lolPath = lolpath;
			this.randomSpell = rndss;
			this.queueType = queuetyp;
			this.replaceConfig = rplccfg;
			this.InitializeComponent();
		}

		private void console_Load(object sender, EventArgs e)
		{
			this.updateStatus(msgStatus.INFO, "Console initialized.", "CONSOLE");
			if (this.replaceConfig)
			{
				this.updateStatus(msgStatus.INFO, "Replacing Config.", "CONSOLE");
				try
				{
					Basic.ReplaceGameConfig(this.lolPath);
				}
				catch (Exception)
				{
				}
			}
			this.updateStatus(msgStatus.INFO, "Loading Accounts", "CONSOLE");
			this.loadAccounts();
			int num = 0;
			foreach (string text in this.accounts)
			{
				try
				{
					this.accountsNew.RemoveAt(0);
					string[] separator = new string[]
					{
						"|"
					};
					string[] array = text.Split(separator, StringSplitOptions.None);
					num++;
					if (array[2] != null)
					{
						QueueTypes queue = (QueueTypes)Enum.Parse(typeof(QueueTypes), array[2]);
						new OldVoliBot(array[0], array[1], this, queue);
					}
					else
					{
						QueueTypes queue2 = QueueTypes.ARAM;
						new OldVoliBot(array[0], array[1], this, queue2);
					}
					if (num == this.maxBots)
					{
						break;
					}
				}
				catch (Exception ex)
				{
					this.updateStatus(msgStatus.ERROR, "错误:你的Accounts.txt里面格式可能错误" + ex.Message, "CONSOLE");
				}
			}
		}

		public void loadAccounts()
		{
			try
			{
				TextReader textReader = File.OpenText(this.accounts_txt_path);
				string value;
				while ((value = textReader.ReadLine()) != null)
				{
					this.accounts.Add(value);
					this.accountsNew.Add(value);
				}
				textReader.Close();
			}
			catch (Exception)
			{
				this.updateStatus(msgStatus.ERROR, "未找到Accounts.txt，请检查你的设置", "CONSOLE");
			}
		}

		public void lognNewAccount()
		{
			this.accountsNew = this.accounts;
			this.accounts.RemoveAt(0);
			int num = 0;
			if (this.accounts.Count == 0)
			{
				this.updateStatus(msgStatus.INFO, "No more accounts to login.", "CONSOLE");
			}
			foreach (string text in this.accounts)
			{
				string text2 = text;
				string[] separator = new string[]
				{
					"|"
				};
				string[] array = text2.Split(separator, StringSplitOptions.None);
				num++;
				if (array[2] != null)
				{
					QueueTypes queue = (QueueTypes)Enum.Parse(typeof(QueueTypes), array[2]);
					new OldVoliBot(array[0], array[1], this, queue);
				}
				else
				{
					QueueTypes queue2 = QueueTypes.ARAM;
					new OldVoliBot(array[0], array[1], this, queue2);
				}
				if (num == this.maxBots)
				{
					break;
				}
			}
		}

		public void updateStatus(msgStatus type, string msg, string _username)
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
			this.richTextBox1.AppendText("[" + DateTime.Now.ToShortTimeString() + "]", Color.Yellow);
			this.richTextBox1.AppendText(" ", Color.Black);
			this.richTextBox1.AppendText(_username, Color.Yellow);
			this.richTextBox1.AppendText(": ", Color.White);
			this.richTextBox1.AppendText(msg, Color.White);
			this.richTextBox1.AppendText(Environment.NewLine, Color.Black);
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
			this.richTextBox1 = new RichTextBox();
			base.SuspendLayout();
			this.richTextBox1.BackColor = SystemColors.ActiveCaptionText;
			this.richTextBox1.BorderStyle = BorderStyle.None;
			this.richTextBox1.Font = new Font("Lucida Console", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.richTextBox1.ForeColor = SystemColors.Info;
			this.richTextBox1.Location = new Point(1, 2);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new Size(502, 276);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.ActiveCaptionText;
			base.ClientSize = new Size(504, 281);
			base.Controls.Add(this.richTextBox1);
			this.DoubleBuffered = true;
			this.ForeColor = SystemColors.ControlLightLight;
			this.MinimumSize = new Size(520, 320);
			base.Name = "console";
			base.ShowIcon = false;
			this.Text = "VoliBot 5.24 Console";
			base.Load += new EventHandler(this.console_Load);
			base.ResumeLayout(false);
		}
	}
}
