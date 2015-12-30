using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VoliBot.Properties;
using VoliBot.Utils;

namespace VoliBot
{
	public class AccountManager : Form
	{
		public static VoliBot _parent;

		public string specificFolder;

		public List<AccountInBox> accountsInBox = new List<AccountInBox>();

		private IContainer components;

		private Label label1;

		private Button button1;

		private TextBox textBox1;

		private ComboBox comboBox1;

		private Label label2;

		private Label label3;

		private TextBox textBox2;

		private Button button2;

		private Button button3;

		private ListBox listBox1;

		private CheckBox checkBox1;

		private Button button4;

		private Button button5;

		public AccountManager(VoliBot parent)
		{
			AccountManager._parent = parent;
			this.InitializeComponent();
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			this.specificFolder = Path.Combine(folderPath, "VoliBot");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			AccountManager._parent.addMdiChild(this.textBox1.Text, this.textBox2.Text, this.comboBox1.Text, false);
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			AccountManager._parent.addMdiChild(this.textBox1.Text, this.textBox2.Text, this.comboBox1.Text, false);
		}

		public void addAccount(string username, string password, string region)
		{
			AccountInBox item = new AccountInBox(username, password, region);
			this.accountsInBox.Add(item);
			string text = "";
			for (int i = 0; i < password.Length; i++)
			{
				text += "•";
			}
			using (StreamWriter streamWriter = new StreamWriter(this.specificFolder + "\\accounts.txt", true))
			{
				streamWriter.WriteLine(string.Concat(new string[]
				{
					username,
					"|",
					password,
					"|",
					region
				}));
			}
			this.listBox1.Items.Add(string.Concat(new string[]
			{
				username,
				"|",
				text,
				"|",
				region
			}));
		}

		private void AccountManager_Load(object sender, EventArgs e)
		{
			ImageList imageList = new ImageList();
			imageList.Images.Add(Resources.add);
			imageList.ImageSize = new Size(16, 16);
			this.button2.ImageList = imageList;
			this.button2.ImageIndex = 0;
			ImageList imageList2 = new ImageList();
			imageList2.Images.Add(Resources.adelete);
			imageList2.ImageSize = new Size(16, 16);
			this.button3.ImageList = imageList2;
			this.button3.ImageIndex = 0;
			if (File.Exists(this.specificFolder + "\\accounts.txt"))
			{
				TextReader textReader = File.OpenText(this.specificFolder + "\\accounts.txt");
				string text;
				while ((text = textReader.ReadLine()) != null)
				{
					string[] array = text.Split(new char[]
					{
						'|'
					});
					AccountInBox item = new AccountInBox(array[0], array[1], array[2]);
					this.accountsInBox.Add(item);
					string text2 = "";
					for (int i = 0; i < array[1].Length; i++)
					{
						text2 += "•";
					}
					this.listBox1.Items.Add(string.Concat(new string[]
					{
						array[0],
						"|",
						text2,
						"|",
						array[2]
					}));
				}
				textReader.Close();
			}
			object[] regions = Enums.regions;
			for (int j = 0; j < regions.Length; j++)
			{
				string item2 = (string)regions[j];
				this.comboBox1.Items.Add(item2);
			}
			this.comboBox1.SelectedIndex = 1;
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void button4_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			AccountManager_ADD accountManager_ADD = new AccountManager_ADD(this);
			accountManager_ADD.ShowDialog();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			ListBox.SelectedObjectCollection selectedItems = this.listBox1.SelectedItems;
			for (int i = selectedItems.Count - 1; i >= 0; i--)
			{
				string text = selectedItems[i].ToString();
				string[] array = text.Split(new char[]
				{
					'|'
				});
				string username = array[0];
				int passwordLength = array[1].Length;
				string region = array[2];
				AccountInBox item = this.accountsInBox.FirstOrDefault((AccountInBox u) => u._username == username && u._region == region && u._password.Length == passwordLength);
				this.accountsInBox.Remove(item);
				this.listBox1.Items.Remove(selectedItems[i]);
			}
			using (StreamWriter streamWriter = new StreamWriter(this.specificFolder + "\\accounts.txt", false))
			{
				foreach (AccountInBox current in this.accountsInBox)
				{
					streamWriter.WriteLine(string.Concat(new string[]
					{
						current._username,
						"|",
						current._password,
						"|",
						current._region
					}));
				}
			}
		}

		private void button5_Click(object sender, EventArgs e)
		{
			ListBox.SelectedObjectCollection selectedItems = this.listBox1.SelectedItems;
			for (int i = selectedItems.Count - 1; i >= 0; i--)
			{
				string text = selectedItems[i].ToString();
				string[] array = text.Split(new char[]
				{
					'|'
				});
				string username = array[0];
				int passwordLength = array[1].Length;
				string region = array[2];
				AccountInBox accountInBox = this.accountsInBox.FirstOrDefault((AccountInBox u) => u._username == username && u._region == region && u._password.Length == passwordLength);
				if (!this.checkBox1.Checked)
				{
					AccountManager._parent.addMdiChild(accountInBox._username, accountInBox._password, accountInBox._region, false);
				}
				else
				{
					AccountManager._parent.addMdiChild(accountInBox._username, accountInBox._password, accountInBox._region, true);
				}
			}
			if (selectedItems.Count > 1)
			{
				base.Close();
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
			this.label1 = new Label();
			this.button1 = new Button();
			this.textBox1 = new TextBox();
			this.comboBox1 = new ComboBox();
			this.label2 = new Label();
			this.label3 = new Label();
			this.textBox2 = new TextBox();
			this.button3 = new Button();
			this.button2 = new Button();
			this.listBox1 = new ListBox();
			this.checkBox1 = new CheckBox();
			this.button4 = new Button();
			this.button5 = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(50, 292);
			this.label1.Name = "label1";
			this.label1.Size = new Size(58, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "账号:";
			this.button1.Location = new Point(53, 414);
			this.button1.Name = "button1";
			this.button1.Size = new Size(206, 32);
			this.button1.TabIndex = 20;
			this.button1.Text = "添加";
			this.button1.UseVisualStyleBackColor = true;
			this.textBox1.BorderStyle = BorderStyle.FixedSingle;
			this.textBox1.Location = new Point(53, 308);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(206, 20);
			this.textBox1.TabIndex = 15;
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(53, 386);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(206, 21);
			this.comboBox1.TabIndex = 19;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(50, 331);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 13);
			this.label2.TabIndex = 16;
			this.label2.Text = "密码:";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(50, 370);
			this.label3.Name = "label3";
			this.label3.Size = new Size(44, 13);
			this.label3.TabIndex = 18;
			this.label3.Text = "服务器:";
			this.textBox2.BorderStyle = BorderStyle.FixedSingle;
			this.textBox2.Location = new Point(53, 347);
			this.textBox2.Name = "textBox2";
			this.textBox2.PasswordChar = '•';
			this.textBox2.Size = new Size(206, 20);
			this.textBox2.TabIndex = 17;
			this.button3.Location = new Point(153, 10);
			this.button3.Name = "button3";
			this.button3.Size = new Size(135, 23);
			this.button3.TabIndex = 22;
			this.button3.Text = "删除账号";
			this.button3.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.button2.Location = new Point(12, 10);
			this.button2.Name = "button2";
			this.button2.Size = new Size(135, 23);
			this.button2.TabIndex = 21;
			this.button2.Text = "添加账号";
			this.button2.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new Point(12, 39);
			this.listBox1.Name = "listBox1";
			this.listBox1.SelectionMode = SelectionMode.MultiSimple;
			this.listBox1.Size = new Size(276, 173);
			this.listBox1.TabIndex = 23;
			this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(12, 218);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(137, 17);
			this.checkBox1.TabIndex = 24;
			this.checkBox1.Text = "自动连接";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.button4.Location = new Point(200, 241);
			this.button4.Name = "button4";
			this.button4.Size = new Size(88, 23);
			this.button4.TabIndex = 26;
			this.button4.Text = "取消/返回";
			this.button4.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new EventHandler(this.button4_Click);
			this.button5.Location = new Point(12, 241);
			this.button5.Name = "button5";
			this.button5.Size = new Size(182, 23);
			this.button5.TabIndex = 25;
			this.button5.Text = "载入账号";
			this.button5.TextImageRelation = TextImageRelation.ImageBeforeText;
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new EventHandler(this.button5_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(301, 275);
			base.Controls.Add(this.button4);
			base.Controls.Add(this.button5);
			base.Controls.Add(this.checkBox1);
			base.Controls.Add(this.listBox1);
			base.Controls.Add(this.button3);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.comboBox1);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.textBox2);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AccountManager";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "账号管理器";
			base.Load += new EventHandler(this.AccountManager_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
