using LoLLauncher;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using VoliBot.Properties;
using VoliBot.Utils;

namespace VoliBot
{
	public class AccountManager_TEST : Form
	{
		private GifImage gifImage;

		private LoLConnection _connection = new LoLConnection();

		private IContainer components;

		private Button button1;

		private Panel panel1;

		private PictureBox pictureBox1;

		private Label label1;

		private Label label2;

		private Timer timer1;

		public AccountManager_TEST(string username, string password, string region)
		{
			this.InitializeComponent();
			this.gifImage = new GifImage(Resources.table);
			this.gifImage.ReverseAtEnd = false;
			this.pictureBox1.Image = this.gifImage.GetFrame(0);
			this.timer1.Enabled = true;
			this._connection = new LoLConnection();
			this._connection.OnLogin += new LoLConnection.OnLoginHandler(this.connection_OnLogin);
			this._connection.OnError += new LoLConnection.OnErrorHandler(this.connection_OnError);
			BaseRegion region2 = BaseRegion.GetRegion(region);
			this._connection.Connect(username, password, region2.PVPRegion, Config.clientSeason + "." + Config.clientSubVersion);
		}

		private void AccountManager_TEST_Load(object sender, EventArgs e)
		{
			this.pictureBox1.Image = Resources.table;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			this.pictureBox1.Image = this.gifImage.GetNextFrame();
		}

		private void connection_OnError(object sender, Error error)
		{
			base.Invoke(new Action(delegate
			{
				this.label1.Text = "Test Result:";
				this.label2.Text = "错误:无法连接此号（请确认服务器，账号密码）";
				this.button1.Enabled = true;
				this._connection.Disconnect();
			}));
		}

		private void connection_OnLogin(object sender, string username, string ipAddress)
		{
			base.Invoke(new Action(delegate
			{
				this.label1.Text = "Test Result:";
				this.label2.Text = "测试登陆成功";
				this.gifImage = new GifImage(Resources.glasses);
				this.button1.Enabled = true;
				this._connection.Disconnect();
			}));
		}

		private static Image GetImageFromURL(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			return Image.FromStream(responseStream);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
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
			this.components = new Container();
			this.button1 = new Button();
			this.panel1 = new Panel();
			this.label1 = new Label();
			this.label2 = new Label();
			this.timer1 = new Timer(this.components);
			this.pictureBox1 = new PictureBox();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.button1.Enabled = false;
			this.button1.Location = new Point(219, 123);
			this.button1.Name = "button1";
			this.button1.Size = new Size(86, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.panel1.BackColor = SystemColors.ControlLight;
			this.panel1.Location = new Point(-3, 109);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(322, 52);
			this.panel1.TabIndex = 1;
			this.label1.AutoSize = true;
			this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
			this.label1.Location = new Point(128, 29);
			this.label1.Name = "label1";
			this.label1.Size = new Size(72, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Testing...";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(128, 55);
			this.label2.Name = "label2";
			this.label2.Size = new Size(159, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Please be patient while testing...";
			this.timer1.Interval = 500;
			this.timer1.Tick += new EventHandler(this.timer1_Tick);
			this.pictureBox1.Image = Resources.table;
			this.pictureBox1.ImageLocation = "https://33.media.tumblr.com/825f063962022082e495236921ede8b9/tumblr_n30xmdzwQb1sx3qw2o1_500.gif";
			this.pictureBox1.Location = new Point(12, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(100, 100);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(317, 158);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.panel1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AccountManager_TEST";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "账号测试";
			base.Load += new EventHandler(this.AccountManager_TEST_Load);
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
