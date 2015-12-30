using LoLLauncher;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VoliBot.Utils;

namespace VoliBot
{
	public class AccountManager_ADD : Form
	{
		protected AccountManager _parent;

		private LoLConnection _connection = new LoLConnection();

		private AccountManager_TEST _testDialog;

		private IContainer components;

		private Button button1;

		private Button button2;

		private Label label1;

		private TextBox textBox1;

		private TextBox textBox2;

		private Label label2;

		private Label label3;

		private ComboBox comboBox1;

		public AccountManager_ADD(AccountManager paps)
		{
			this._parent = paps;
			this.InitializeComponent();
		}

		private void AccountManager_ADD_Load(object sender, EventArgs e)
		{
			object[] regions = Enums.regions;
			for (int i = 0; i < regions.Length; i++)
			{
				string text = (string)regions[i];
				this.comboBox1.Items.Add(text);
				if (text == Config.defaultRegion)
				{
					this.comboBox1.SelectedItem = text;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this._parent.addAccount(this.textBox1.Text, this.textBox2.Text, this.comboBox1.Text);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this._testDialog = new AccountManager_TEST(this.textBox1.Text, this.textBox2.Text, this.comboBox1.Text);
			this._testDialog.ShowDialog();
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
			this.button1 = new Button();
			this.button2 = new Button();
			this.label1 = new Label();
			this.textBox1 = new TextBox();
			this.textBox2 = new TextBox();
			this.label2 = new Label();
			this.label3 = new Label();
			this.comboBox1 = new ComboBox();
			base.SuspendLayout();
			this.button1.Location = new Point(120, 136);
			this.button1.Name = "button1";
			this.button1.Size = new Size(128, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "添加";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(12, 136);
			this.button2.Name = "button2";
			this.button2.Size = new Size(102, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "测试账号";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new EventHandler(this.button2_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(55, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "账号";
			this.textBox1.Location = new Point(12, 25);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(236, 20);
			this.textBox1.TabIndex = 3;
			this.textBox2.Location = new Point(12, 64);
			this.textBox2.Name = "textBox2";
			this.textBox2.PasswordChar = '•';
			this.textBox2.Size = new Size(236, 20);
			this.textBox2.TabIndex = 5;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(9, 48);
			this.label2.Name = "label2";
			this.label2.Size = new Size(53, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "密码";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(9, 87);
			this.label3.Name = "label3";
			this.label3.Size = new Size(41, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "服务器";
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(12, 103);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(236, 21);
			this.comboBox1.TabIndex = 7;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(260, 167);
			base.Controls.Add(this.comboBox1);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AccountManager_ADD";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "添加账号";
			base.Load += new EventHandler(this.AccountManager_ADD_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
