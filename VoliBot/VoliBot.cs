using Ini;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using VoliBot.Properties;
using VoliBot.Utils;
using VoliBots;

namespace VoliBot
{
	public class VoliBot : Form
	{
		public static List<vClient> mdiChilds = new List<vClient>();

		public static string specificFolder;

		private int id = 1;

		private IContainer components;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem accountsToolStripMenuItem;

		private StatusStrip statusStrip1;

		private ToolStripMenuItem donateToolStripMenuItem;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private ToolStripStatusLabel toolStripStatusLabel2;

		private exListBox exListBox1;

		private ToolStripMenuItem championsToolStripMenuItem;

		private ToolStripMenuItem groupsToolStripMenuItem;

		private ToolStripMenuItem startVoliBotOLDToolStripMenuItem;

		public VoliBot()
		{
			this.InitializeComponent();
			this.DoubleBuffered = true;
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			ToolStripSeparator value = new ToolStripSeparator();
			ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
			toolStripSeparator.Alignment = ToolStripItemAlignment.Right;
			this.menuStrip1.Items.Add(value);
			this.menuStrip1.Items.Add(toolStripSeparator);
		}

		private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AccountManager accountManager = new AccountManager(this);
			accountManager.ShowDialog();
		}

		internal void onlyUpdateListItemStatusAndLevel(exListBoxItem elbi, string newStatus, string newLevel)
		{
			exListBoxItem exListBoxItem = (exListBoxItem)this.exListBox1.Items[this.exListBox1.Items.IndexOf(elbi)];
			exListBoxItem.Details = newStatus;
			exListBoxItem.Level = newLevel;
			this.exListBox1.Refresh();
		}

		internal void updateListItem(exListBoxItem elbi, string newName, string newStatus, string newLevel, Image newImage)
		{
			exListBoxItem exListBoxItem = (exListBoxItem)this.exListBox1.Items[this.exListBox1.Items.IndexOf(elbi)];
			exListBoxItem.Title = newName;
			exListBoxItem.Details = newStatus;
			exListBoxItem.Level = newLevel;
			exListBoxItem.ItemImage = newImage;
			this.exListBox1.Refresh();
		}

		internal void updateListItemImage(exListBoxItem elbi, Image newImage)
		{
			exListBoxItem exListBoxItem = (exListBoxItem)this.exListBox1.Items[this.exListBox1.Items.IndexOf(elbi)];
			exListBoxItem.ItemImage = newImage;
			this.exListBox1.Refresh();
		}

		public void addMdiChild(string username, string password, string region, bool autoConnect = false)
		{
			if (VoliBot.mdiChilds.Count((vClient u) => u._username == username) > 0)
			{
				MessageBox.Show("This Client already exist.");
				return;
			}
			Image image = Basic.returnIcon(0);
			vClient vClient = new vClient(username, password, region, this, autoConnect);
			exListBoxItem exListBoxItem = new exListBoxItem(vClient._username, vClient._username, "Status: Waiting...", "", image);
			vClient.addListBoxItem(exListBoxItem);
			this.exListBox1.Items.Add(exListBoxItem);
			VoliBot.mdiChilds.Add(vClient);
			vClient.MdiParent = this;
			vClient.Show();
			this.id++;
		}

		internal void removeMdiChild(string username, exListBoxItem elbi)
		{
			vClient item = VoliBot.mdiChilds.FirstOrDefault((vClient u) => u._username == username);
			try
			{
				this.exListBox1.Items.Remove(elbi);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error on remove Child. Please contact me with the Message blow!\n\n" + ex.Message);
			}
			VoliBot.mdiChilds.Remove(item);
		}

		private void VoliBot_Load(object sender, EventArgs e)
		{
			new WelcomeWindow(this, false)
			{
				MdiParent = this
			}.Show();
		}

		private void LoadConfigs()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			VoliBot.specificFolder = Path.Combine(folderPath, "VoliBot");
			if (File.Exists(VoliBot.specificFolder + "\\config.ini"))
			{
				IniFile iniFile = new IniFile(VoliBot.specificFolder + "\\config.ini");
				Config.defaultPath = iniFile.IniReadValue("General", "LauncherPath");
				Config.defaultRegion = iniFile.IniReadValue("General", "DefaultRegion");
				Config.defaultSlotOne = iniFile.IniReadValue("General", "DefaultSpell1");
				Config.defaultSlotTwo = iniFile.IniReadValue("General", "DefaultSpell2");
				Config.defaultQueue = iniFile.IniReadValue("General", "DefaultQueue");
				Config.defaultChampion = iniFile.IniReadValue("General", "DefaultChampion");
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			foreach (Control control in base.Controls)
			{
				if (control is MdiClient)
				{
					this.BackgroundImageLayout = ImageLayout.Center;
					control.BackgroundImage = Resources.dayum;
					break;
				}
			}
			base.OnLoad(e);
		}

		public void accpetedAgreement()
		{
			this.accountsToolStripMenuItem.Enabled = true;
			this.startVoliBotOLDToolStripMenuItem.Enabled = true;
			this.championsToolStripMenuItem.Enabled = true;
		}

		private static Image GetImageFromURL(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			return Image.FromStream(responseStream);
		}

		private void exListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void exListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				int num = this.exListBox1.IndexFromPoint(e.Location);
				if (num != -1)
				{
					exListBoxItem ix = (exListBoxItem)this.exListBox1.Items[num];
					vClient vClient = VoliBot.mdiChilds.FirstOrDefault((vClient u) => u._username == ix.Id);
					if (vClient.WindowState == FormWindowState.Maximized)
					{
						vClient.WindowState = FormWindowState.Normal;
					}
					else
					{
						vClient.WindowState = FormWindowState.Maximized;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void donateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("http://volibot.com/#our-team");
		}

		private void VoliBot_FormClosed(object sender, FormClosedEventArgs e)
		{
			foreach (vClient current in VoliBot.mdiChilds)
			{
				try
				{
					current.killContainedLeague();
				}
				catch (Exception)
				{
				}
			}
		}

		private void championsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new WelcomeWindow(this, true)
			{
				MdiParent = this
			}.Show();
		}

		private void groupsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("就让你无视了还点。。。。");
		}

		private void startVoliBotOLDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new old_volibot(this)
			{
				MdiParent = this
			}.Show();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(VoliBot));
			this.menuStrip1 = new MenuStrip();
			this.accountsToolStripMenuItem = new ToolStripMenuItem();
			this.donateToolStripMenuItem = new ToolStripMenuItem();
			this.championsToolStripMenuItem = new ToolStripMenuItem();
			this.groupsToolStripMenuItem = new ToolStripMenuItem();
			this.startVoliBotOLDToolStripMenuItem = new ToolStripMenuItem();
			this.statusStrip1 = new StatusStrip();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new ToolStripStatusLabel();
			this.exListBox1 = new exListBox();
			this.menuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			base.SuspendLayout();
			this.menuStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.accountsToolStripMenuItem,
				this.donateToolStripMenuItem,
				this.championsToolStripMenuItem,
				this.groupsToolStripMenuItem,
				this.startVoliBotOLDToolStripMenuItem
			});
			this.menuStrip1.Location = new Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = ToolStripRenderMode.Professional;
			this.menuStrip1.Size = new Size(954, 39);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			this.accountsToolStripMenuItem.Enabled = false;
			this.accountsToolStripMenuItem.Image = Resources.profile7;
			this.accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
			this.accountsToolStripMenuItem.Size = new Size(69, 35);
			this.accountsToolStripMenuItem.Text = "账户♥";
			this.accountsToolStripMenuItem.TextImageRelation = TextImageRelation.ImageAboveText;
			this.accountsToolStripMenuItem.Click += new EventHandler(this.accountsToolStripMenuItem_Click);
			this.donateToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
			this.donateToolStripMenuItem.Image = Resources.kings;
			this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
			this.donateToolStripMenuItem.Size = new Size(57, 35);
			this.donateToolStripMenuItem.Text = "官网（已过期）";
			this.donateToolStripMenuItem.TextImageRelation = TextImageRelation.ImageAboveText;
			this.donateToolStripMenuItem.Click += new EventHandler(this.donateToolStripMenuItem_Click);
			this.championsToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
			this.championsToolStripMenuItem.Enabled = false;
			this.championsToolStripMenuItem.Image = Resources.cogwheel8;
			this.championsToolStripMenuItem.Name = "championsToolStripMenuItem";
			this.championsToolStripMenuItem.Size = new Size(96, 35);
			this.championsToolStripMenuItem.Text = "基本设置";
			this.championsToolStripMenuItem.TextImageRelation = TextImageRelation.ImageAboveText;
			this.championsToolStripMenuItem.Click += new EventHandler(this.championsToolStripMenuItem_Click);
			this.groupsToolStripMenuItem.Image = Resources.user73;
			this.groupsToolStripMenuItem.Name = "groupsToolStripMenuItem";
			this.groupsToolStripMenuItem.Size = new Size(87, 35);
			this.groupsToolStripMenuItem.Text = "无视这个";
			this.groupsToolStripMenuItem.TextImageRelation = TextImageRelation.ImageAboveText;
			this.groupsToolStripMenuItem.Click += new EventHandler(this.groupsToolStripMenuItem_Click);
			this.startVoliBotOLDToolStripMenuItem.Enabled = false;
			this.startVoliBotOLDToolStripMenuItem.Image = Resources.personal5;
			this.startVoliBotOLDToolStripMenuItem.Name = "startVoliBotOLDToolStripMenuItem";
			this.startVoliBotOLDToolStripMenuItem.Size = new Size(83, 35);
			this.startVoliBotOLDToolStripMenuItem.Text = "命令行版本挂机";
			this.startVoliBotOLDToolStripMenuItem.TextImageRelation = TextImageRelation.ImageAboveText;
			this.startVoliBotOLDToolStripMenuItem.Click += new EventHandler(this.startVoliBotOLDToolStripMenuItem_Click);
			this.statusStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabel1,
				this.toolStripStatusLabel2
			});
			this.statusStrip1.Location = new Point(0, 646);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new Size(954, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new Size(90, 17);
			this.toolStripStatusLabel1.Text = "大家可以使用L#，BOL，EB脚本配合挂机，点“基本设置”左边f图标可看教程";
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new Size(29, 17);
			this.toolStripStatusLabel2.Text = "不知道什么脚本可以论坛私信我，脑残及伸手党退散，谢谢。第一次使用会崩溃是正常！";
			this.exListBox1.BorderStyle = BorderStyle.FixedSingle;
			this.exListBox1.Dock = DockStyle.Left;
			this.exListBox1.DrawMode = DrawMode.OwnerDrawVariable;
			this.exListBox1.FormattingEnabled = true;
			this.exListBox1.ItemHeight = 66;
			this.exListBox1.Location = new Point(0, 39);
			this.exListBox1.Margin = new Padding(0);
			this.exListBox1.Name = "exListBox1";
			this.exListBox1.Size = new Size(186, 607);
			this.exListBox1.TabIndex = 9;
			this.exListBox1.SelectedIndexChanged += new EventHandler(this.exListBox1_SelectedIndexChanged);
			this.exListBox1.MouseDoubleClick += new MouseEventHandler(this.exListBox1_MouseDoubleClick);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(954, 668);
			base.Controls.Add(this.exListBox1);
			base.Controls.Add(this.statusStrip1);
			base.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.IsMdiContainer = true;
			base.MainMenuStrip = this.menuStrip1;
			base.Name = "VoliBot";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "VoliBot 5.24 界面版 由LOVETAIWAN更新汉化";
			base.FormClosed += new FormClosedEventHandler(this.VoliBot_FormClosed);
			base.Load += new EventHandler(this.VoliBot_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
