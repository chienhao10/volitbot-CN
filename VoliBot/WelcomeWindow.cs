using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VoliBot.Properties;
using VoliBot.Utils;

namespace VoliBot
{
	public class WelcomeWindow : Form
	{
		public VoliBot _parent;

		private string specificFolder;

		private bool _forConfig;

		private IContainer components;

		private Label label1;

		private PictureBox pictureBox1;

		private PictureBox pictureBox2;

		private Panel panel1;

		private Button button1;

		private Button button2;

		private RichTextBox richTextBox1;

		private PictureBox pictureBox3;

		private PictureBox pictureBox4;

		private PictureBox pictureBox5;

		private Label label2;

		private Label label3;

		private TabControl tabControl1;

		private TabPage tabPage1;

		private TabPage tabPage2;

		private GroupBox groupBox1;

		private ComboBox comboBox4;

		private Label label9;

		private ComboBox comboBox3;

		private Label label8;

		private ComboBox comboBox2;

		private Label label7;

		private ComboBox comboBox1;

		private Label label6;

		private Button button3;

		private TextBox textBox1;

		private Label label5;

		private RichTextBox richTextBox2;

		private Label label4;

		private Button button6;

		private Button button4;

		private Button button5;

		private ComboBox comboBox5;

		private Label label10;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (!this._forConfig)
				{
					createParams.ClassStyle |= 512;
				}
				return createParams;
			}
		}

		public WelcomeWindow(VoliBot parent, bool isOnlyForConfig = false)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			this.specificFolder = Path.Combine(folderPath, "VoliBot");
			this._parent = parent;
			this._forConfig = isOnlyForConfig;
			this.InitializeComponent();
		}

		private void WelcomeWindow_Load(object sender, EventArgs e)
		{
			object[] regions = Enums.regions;
			for (int i = 0; i < regions.Length; i++)
			{
				string item = (string)regions[i];
				this.comboBox1.Items.Add(item);
			}
			this.comboBox1.SelectedIndex = 0;
			object[] spells = Enums.spells;
			for (int j = 0; j < spells.Length; j++)
			{
				string item2 = (string)spells[j];
				this.comboBox2.Items.Add(item2);
				this.comboBox3.Items.Add(item2);
			}
			this.comboBox2.SelectedIndex = 0;
			this.comboBox3.SelectedIndex = 0;
			object[] queues = Enums.queues;
			for (int k = 0; k < queues.Length; k++)
			{
				string item3 = (string)queues[k];
				this.comboBox4.Items.Add(item3);
			}
			this.comboBox4.SelectedIndex = 0;
			this.comboBox5.Items.Add("RANDOM");
			object[] champions = Enums.champions;
			for (int l = 0; l < champions.Length; l++)
			{
				string value = (string)champions[l];
				this.comboBox5.Items.Add(Basic.UppercaseFirst(value));
			}
			this.comboBox5.SelectedIndex = 0;
			if (this._forConfig)
			{
				this.button6.Text = "关闭";
				this.tabControl1.SelectTab(1);
				this.label4.Text = "基础设置";
				this.button6.Click += new EventHandler(this.button6_alternate);
				this.textBox1.Text = Config.defaultPath;
				this.Text = "全局 / 基本设置";
				this.richTextBox2.Text = "不会用的话点左边蓝色背景白色f的图标（facebook）看教程";
				for (int m = 0; m < this.comboBox1.Items.Count; m++)
				{
					if (this.comboBox1.GetItemText(this.comboBox1.Items[m]) == Config.defaultRegion)
					{
						this.comboBox1.SelectedIndex = m;
					}
				}
				for (int n = 0; n < this.comboBox2.Items.Count; n++)
				{
					if (this.comboBox2.GetItemText(this.comboBox2.Items[n]) == Config.defaultSlotOne)
					{
						this.comboBox2.SelectedIndex = n;
					}
				}
				for (int num = 0; num < this.comboBox3.Items.Count; num++)
				{
					if (this.comboBox3.GetItemText(this.comboBox3.Items[num]) == Config.defaultSlotTwo)
					{
						this.comboBox3.SelectedIndex = num;
					}
				}
				for (int num2 = 0; num2 < this.comboBox4.Items.Count; num2++)
				{
					if (this.comboBox4.GetItemText(this.comboBox4.Items[num2]) == Config.defaultQueue)
					{
						this.comboBox4.SelectedIndex = num2;
					}
				}
				for (int num3 = 0; num3 < this.comboBox5.Items.Count; num3++)
				{
					if (this.comboBox5.GetItemText(this.comboBox5.Items[num3]) == Config.defaultChampion)
					{
						this.comboBox5.SelectedIndex = num3;
					}
				}
			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.elobuddy.net/topic/10288-volibot%E6%8C%82%E6%9C%BA%E6%B1%89%E5%8C%96%E7%89%88%EF%BC%88gui%E7%89%88%E6%9C%AC%E5%91%BD%E4%BB%A4%E8%A1%8C%E7%89%88%E6%9C%AC%EF%BC%89%EF%BC%88%E6%8C%82%E6%9C%BA%E6%95%99%E7%A8%8B%E5%9B%BE%E6%96%87%EF%BC%89/");
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Thanks for your interesst in our Twitter-Network. But it currently doesn't exist.");
		}

		private void pictureBox5_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Thanks for your interesst in our Videos. But they currently doesn't exist.");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (!File.Exists(this.specificFolder + "\\config.ini"))
			{
				MessageBox.Show(this.specificFolder + "\\config.ini");
				this.tabControl1.SelectTab(1);
				return;
			}
			this._parent.accpetedAgreement();
			base.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MessageBox.Show("都叫你别点这个了。");
			Application.Exit();
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Wow! Thanks!\nBut please Note: If you only want to donate less than 0,50€ please keep it, I'll get 0,00€ because of the PayPal fees, still thank you!");
			Process.Start("http://volibot.com/#our-team");
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (this._forConfig)
			{
				this.textBox1.Text = Config.defaultPath;
				for (int i = 0; i < this.comboBox1.Items.Count; i++)
				{
					if (this.comboBox1.GetItemText(this.comboBox1.Items[i]) == Config.defaultRegion)
					{
						this.comboBox1.SelectedIndex = i;
					}
				}
				for (int j = 0; j < this.comboBox2.Items.Count; j++)
				{
					if (this.comboBox2.GetItemText(this.comboBox2.Items[j]) == Config.defaultSlotOne)
					{
						this.comboBox2.SelectedIndex = j;
					}
				}
				for (int k = 0; k < this.comboBox3.Items.Count; k++)
				{
					if (this.comboBox3.GetItemText(this.comboBox3.Items[k]) == Config.defaultSlotTwo)
					{
						this.comboBox3.SelectedIndex = k;
					}
				}
				for (int l = 0; l < this.comboBox4.Items.Count; l++)
				{
					if (this.comboBox4.GetItemText(this.comboBox4.Items[l]) == Config.defaultQueue)
					{
						this.comboBox4.SelectedIndex = l;
					}
				}
				for (int m = 0; m < this.comboBox5.Items.Count; m++)
				{
					if (this.comboBox5.GetItemText(this.comboBox5.Items[m]) == Config.defaultChampion)
					{
						this.comboBox5.SelectedIndex = m;
					}
				}
				return;
			}
			this.textBox1.Text = "";
			this.comboBox1.SelectedIndex = 0;
			this.comboBox2.SelectedIndex = 0;
			this.comboBox3.SelectedIndex = 0;
			this.comboBox4.SelectedIndex = 0;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			this.tabControl1.SelectTab(0);
		}

		private void button6_alternate(object sender, EventArgs e)
		{
			base.Close();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			string path = this.specificFolder + "\\config.ini";
			if (!Directory.Exists(this.specificFolder))
			{
				Directory.CreateDirectory(this.specificFolder);
			}
			if (!File.Exists(path))
			{
				using (StreamWriter streamWriter = File.CreateText(path))
				{
					streamWriter.Write(string.Concat(new string[]
					{
						"[General]",
						Environment.NewLine,
						"LauncherPath=",
						this.textBox1.Text,
						Environment.NewLine,
						"DefaultRegion=",
						this.comboBox1.SelectedItem.ToString(),
						Environment.NewLine,
						"DefaultSpell1=",
						this.comboBox2.SelectedItem.ToString(),
						Environment.NewLine,
						"DefaultSpell2=",
						this.comboBox3.SelectedItem.ToString(),
						Environment.NewLine,
						"DefaultQueue=",
						this.comboBox4.SelectedItem.ToString(),
						Environment.NewLine,
						"DefaultChampion=",
						this.comboBox5.SelectedItem.ToString()
					}));
					goto IL_259;
				}
			}
			using (StreamWriter streamWriter2 = File.CreateText(path))
			{
				streamWriter2.Write(string.Concat(new string[]
				{
					"[General]",
					Environment.NewLine,
					"LauncherPath=",
					this.textBox1.Text,
					Environment.NewLine,
					"DefaultRegion=",
					this.comboBox1.SelectedItem.ToString(),
					Environment.NewLine,
					"DefaultSpell1=",
					this.comboBox2.SelectedItem.ToString(),
					Environment.NewLine,
					"DefaultSpell2=",
					this.comboBox3.SelectedItem.ToString(),
					Environment.NewLine,
					"DefaultQueue=",
					this.comboBox4.SelectedItem.ToString(),
					Environment.NewLine,
					"DefaultChampion=",
					this.comboBox5.SelectedItem.ToString()
				}));
			}
			IL_259:
			Config.defaultPath = this.textBox1.Text;
			Config.defaultRegion = this.comboBox1.SelectedItem.ToString();
			Config.defaultSlotOne = this.comboBox2.SelectedItem.ToString();
			Config.defaultSlotTwo = this.comboBox3.SelectedItem.ToString();
			Config.defaultQueue = this.comboBox4.SelectedItem.ToString();
			Config.defaultChampion = this.comboBox5.SelectedItem.ToString();
			if (this._forConfig)
			{
				MessageBox.Show(Config.defaultPath);
			}
			else
			{
				base.Close();
			}
			this._parent.accpetedAgreement();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(WelcomeWindow));
			this.label1 = new Label();
			this.panel1 = new Panel();
			this.button1 = new Button();
			this.button2 = new Button();
			this.richTextBox1 = new RichTextBox();
			this.label2 = new Label();
			this.label3 = new Label();
			this.tabControl1 = new TabControl();
			this.tabPage1 = new TabPage();
			this.tabPage2 = new TabPage();
			this.groupBox1 = new GroupBox();
			this.comboBox5 = new ComboBox();
			this.label10 = new Label();
			this.comboBox4 = new ComboBox();
			this.label9 = new Label();
			this.comboBox3 = new ComboBox();
			this.label8 = new Label();
			this.comboBox2 = new ComboBox();
			this.label7 = new Label();
			this.comboBox1 = new ComboBox();
			this.label6 = new Label();
			this.button3 = new Button();
			this.textBox1 = new TextBox();
			this.label5 = new Label();
			this.button6 = new Button();
			this.button4 = new Button();
			this.button5 = new Button();
			this.richTextBox2 = new RichTextBox();
			this.label4 = new Label();
			this.pictureBox5 = new PictureBox();
			this.pictureBox4 = new PictureBox();
			this.pictureBox3 = new PictureBox();
			this.pictureBox2 = new PictureBox();
			this.pictureBox1 = new PictureBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.pictureBox5).BeginInit();
			((ISupportInitialize)this.pictureBox4).BeginInit();
			((ISupportInitialize)this.pictureBox3).BeginInit();
			((ISupportInitialize)this.pictureBox2).BeginInit();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label1.Location = new Point(15, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(224, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "欢迎使用VOLIBOT!";
			this.panel1.BackColor = SystemColors.ActiveCaptionText;
			this.panel1.Location = new Point(163, 4);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(2, 290);
			this.panel1.TabIndex = 3;
			this.button1.Location = new Point(127, 268);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "别点这个";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(46, 268);
			this.button2.Name = "button2";
			this.button2.Size = new Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "点这个";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.richTextBox1.BackColor = SystemColors.Control;
			this.richTextBox1.BorderStyle = BorderStyle.None;
			this.richTextBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.richTextBox1.Location = new Point(6, 53);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new Size(236, 199);
			this.richTextBox1.TabIndex = 6;
			this.richTextBox1.Text = componentResourceManager.GetString("richTextBox1.Text");
			this.richTextBox1.TextChanged += new EventHandler(this.richTextBox1_TextChanged);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(5, 212);
			this.label2.Name = "label2";
			this.label2.Size = new Size(144, 26);
			this.label2.TabIndex = 11;
			this.label2.Text = "点上面图标有图文教程 :)";
			this.label3.AutoSize = true;
			this.label3.ForeColor = SystemColors.ControlDarkDark;
			this.label3.Location = new Point(47, 128);
			this.label3.Name = "label3";
			this.label3.Size = new Size(65, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "挂机软件 LOVETAIWAN汉化";
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new Point(162, -22);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new Size(259, 331);
			this.tabControl1.TabIndex = 14;
			this.tabPage1.BackColor = SystemColors.Control;
			this.tabPage1.Controls.Add(this.richTextBox1);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.button1);
			this.tabPage1.Controls.Add(this.button2);
			this.tabPage1.Location = new Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new Padding(3);
			this.tabPage1.Size = new Size(251, 305);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage2.BackColor = SystemColors.Control;
			this.tabPage2.Controls.Add(this.groupBox1);
			this.tabPage2.Controls.Add(this.button6);
			this.tabPage2.Controls.Add(this.button4);
			this.tabPage2.Controls.Add(this.button5);
			this.tabPage2.Controls.Add(this.richTextBox2);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Location = new Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new Padding(3);
			this.tabPage2.Size = new Size(251, 305);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.groupBox1.Controls.Add(this.comboBox5);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.comboBox4);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.comboBox3);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.comboBox2);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.groupBox1.Location = new Point(6, 76);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(242, 186);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "基本设置";
			this.comboBox5.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox5.FlatStyle = FlatStyle.System;
			this.comboBox5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.comboBox5.FormattingEnabled = true;
			this.comboBox5.Location = new Point(8, 157);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new Size(219, 21);
			this.comboBox5.TabIndex = 12;
			this.label10.AutoSize = true;
			this.label10.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label10.Location = new Point(5, 141);
			this.label10.Name = "label10";
			this.label10.Size = new Size(90, 13);
			this.label10.TabIndex = 11;
			this.label10.Text = "英雄选择";
			this.label10.TextAlign = ContentAlignment.TopCenter;
			this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox4.FlatStyle = FlatStyle.System;
			this.comboBox4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Location = new Point(9, 115);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new Size(219, 21);
			this.comboBox4.TabIndex = 10;
			this.label9.AutoSize = true;
			this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label9.Location = new Point(6, 99);
			this.label9.Name = "label9";
			this.label9.Size = new Size(66, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "游戏选择（人机？）";
			this.label9.TextAlign = ContentAlignment.TopCenter;
			this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox3.FlatStyle = FlatStyle.System;
			this.comboBox3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new Point(151, 73);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new Size(80, 21);
			this.comboBox3.TabIndex = 8;
			this.label8.AutoSize = true;
			this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label8.Location = new Point(148, 57);
			this.label8.Name = "label8";
			this.label8.Size = new Size(60, 13);
			this.label8.TabIndex = 7;
			this.label8.Text = "召唤师技能2";
			this.label8.TextAlign = ContentAlignment.TopCenter;
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox2.FlatStyle = FlatStyle.System;
			this.comboBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new Point(65, 73);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new Size(80, 21);
			this.comboBox2.TabIndex = 6;
			this.label7.AutoSize = true;
			this.label7.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label7.Location = new Point(62, 58);
			this.label7.Name = "label7";
			this.label7.Size = new Size(60, 13);
			this.label7.TabIndex = 5;
			this.label7.Text = "召唤师技能2";
			this.label7.TextAlign = ContentAlignment.TopCenter;
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FlatStyle = FlatStyle.System;
			this.comboBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(8, 73);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(51, 21);
			this.comboBox1.TabIndex = 4;
			this.label6.AutoSize = true;
			this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label6.Location = new Point(5, 58);
			this.label6.Name = "label6";
			this.label6.Size = new Size(41, 13);
			this.label6.TabIndex = 3;
			this.label6.Text = "地区";
			this.label6.TextAlign = ContentAlignment.TopCenter;
			this.button3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.button3.Location = new Point(206, 33);
			this.button3.Name = "button3";
			this.button3.Size = new Size(25, 23);
			this.button3.TabIndex = 2;
			this.button3.Text = "...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.textBox1.BorderStyle = BorderStyle.FixedSingle;
			this.textBox1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.textBox1.Location = new Point(9, 34);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(191, 20);
			this.textBox1.TabIndex = 1;
			this.textBox1.MouseDoubleClick += new MouseEventHandler(this.button3_Click);
			this.label5.AutoSize = true;
			this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.label5.Location = new Point(6, 18);
			this.label5.Name = "label5";
			this.label5.Size = new Size(131, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "英雄联盟游戏目录";
			this.button6.Location = new Point(194, 268);
			this.button6.Name = "button6";
			this.button6.Size = new Size(44, 23);
			this.button6.TabIndex = 15;
			this.button6.Text = "返回";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new EventHandler(this.button6_Click);
			this.button4.Location = new Point(131, 268);
			this.button4.Name = "button4";
			this.button4.Size = new Size(57, 23);
			this.button4.TabIndex = 14;
			this.button4.Text = "重置";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new EventHandler(this.button4_Click);
			this.button5.Location = new Point(10, 268);
			this.button5.Name = "button5";
			this.button5.Size = new Size(115, 23);
			this.button5.TabIndex = 13;
			this.button5.Text = "保存";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new EventHandler(this.button5_Click);
			this.richTextBox2.BackColor = SystemColors.Control;
			this.richTextBox2.BorderStyle = BorderStyle.None;
			this.richTextBox2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.richTextBox2.Location = new Point(12, 39);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ReadOnly = true;
			this.richTextBox2.Size = new Size(236, 33);
			this.richTextBox2.TabIndex = 7;
			this.richTextBox2.TabStop = false;
			this.richTextBox2.Text = "第一次使用的话建议你设置VOLIBOT！";
			this.label4.AutoSize = true;
			this.label4.Font = new Font("Microsoft Sans Serif", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label4.Location = new Point(31, 8);
			this.label4.Name = "label4";
			this.label4.Size = new Size(208, 25);
			this.label4.TabIndex = 1;
			this.label4.Text = "第一次使用吗？";
			this.label4.TextAlign = ContentAlignment.MiddleCenter;
			this.pictureBox5.Cursor = Cursors.Hand;
			this.pictureBox5.Image = Resources.tube_hex_icon;
			this.pictureBox5.Location = new Point(107, 140);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new Size(45, 45);
			this.pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox5.TabIndex = 10;
			this.pictureBox5.TabStop = false;
			this.pictureBox5.Visible = false;
			this.pictureBox5.Click += new EventHandler(this.pictureBox5_Click);
			this.pictureBox4.Cursor = Cursors.Hand;
			this.pictureBox4.Image = Resources.twitter_hex_icon;
			this.pictureBox4.Location = new Point(56, 140);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new Size(45, 45);
			this.pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox4.TabIndex = 9;
			this.pictureBox4.TabStop = false;
			this.pictureBox4.Visible = false;
			this.pictureBox4.Click += new EventHandler(this.pictureBox4_Click);
			this.pictureBox3.Cursor = Cursors.Hand;
			this.pictureBox3.Image = Resources.facebook_hex_icon;
			this.pictureBox3.Location = new Point(56, 155);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new Size(45, 45);
			this.pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
			this.pictureBox3.TabIndex = 8;
			this.pictureBox3.TabStop = false;
			this.pictureBox3.Click += new EventHandler(this.pictureBox3_Click);
			this.pictureBox2.Cursor = Cursors.Hand;
			this.pictureBox2.ErrorImage = Resources.disconnect;
			this.pictureBox2.Image = Resources.btn_donateCC_LG;
			this.pictureBox2.ImageLocation = "";
			this.pictureBox2.Location = new Point(5, 246);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new Size(147, 47);
			this.pictureBox2.TabIndex = 2;
			this.pictureBox2.TabStop = false;
			this.pictureBox2.Click += new EventHandler(this.pictureBox2_Click);
			this.pictureBox1.Image = Resources.icon;
			this.pictureBox1.Location = new Point(14, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(128, 128);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(418, 305);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.pictureBox5);
			base.Controls.Add(this.pictureBox3);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.pictureBox2);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.tabControl1);
			base.Controls.Add(this.pictureBox4);
			this.DoubleBuffered = true;
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "WelcomeWindow";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "欢迎召唤师使用 VoliBot 5.24 图形界面版~";
			base.Load += new EventHandler(this.WelcomeWindow_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.pictureBox5).EndInit();
			((ISupportInitialize)this.pictureBox4).EndInit();
			((ISupportInitialize)this.pictureBox3).EndInit();
			((ISupportInitialize)this.pictureBox2).EndInit();
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
