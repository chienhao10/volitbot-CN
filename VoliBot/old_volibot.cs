using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VoliBot.Utils;

namespace VoliBot
{
	public class old_volibot : Form
	{
		private VoliBot parent;

		private IContainer components;

		private Button button1;

		private Button button2;

		private Label label1;

		private TextBox textBox1;

		private TextBox textBox2;

		private Label label2;

		private Button button3;

		private Label label3;

		private NumericUpDown numericUpDown1;

		private Label label4;

		private NumericUpDown numericUpDown2;

		private Label label5;

		private ComboBox comboBox1;

		private Label label6;

		private TextBox textBox3;

		private Label label7;

		private ComboBox comboBox2;

		private ComboBox comboBox3;

		private GroupBox groupBox1;

		private CheckBox checkBox1;

		private CheckBox checkBox2;

		private TextBox textBox4;

		private Label label8;

		private Button button4;

		private ComboBox comboBox4;

		private Label label9;

		public old_volibot(VoliBot _parent)
		{
			this.parent = _parent;
			this.InitializeComponent();
		}

		private void old_volibot_Load(object sender, EventArgs e)
		{
			this.textBox1.Text = Config.clientSeason;
			this.textBox4.Text = Config.defaultPath;
			object[] regions = Enums.regions;
			for (int i = 0; i < regions.Length; i++)
			{
				string item = (string)regions[i];
				this.comboBox1.Items.Add(item);
			}
			this.comboBox1.SelectedIndex = 1;
			object[] spells = Enums.spells;
			for (int j = 0; j < spells.Length; j++)
			{
				string item2 = (string)spells[j];
				this.comboBox2.Items.Add(item2);
				this.comboBox3.Items.Add(item2);
			}
			this.comboBox2.SelectedIndex = 0;
			this.comboBox3.SelectedIndex = 1;
			object[] queues = Enums.queues;
			for (int k = 0; k < queues.Length; k++)
			{
				string item3 = (string)queues[k];
				this.comboBox4.Items.Add(item3);
			}
			this.comboBox3.SelectedIndex = 4;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "accounts.txt|accounts.txt";
			openFileDialog.Filter = "Other txt file|*.txt";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				this.textBox2.Text = fileName;
				return;
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			new console(this.textBox1.Text, this.textBox2.Text, Convert.ToInt32(this.numericUpDown1.Value), Convert.ToInt32(this.numericUpDown2.Value), this.comboBox1.Text, this.textBox3.Text, this.comboBox2.Text, this.comboBox3.Text, this.checkBox1.Checked, this.checkBox2.Checked, this.textBox4.Text, this.textBox4.SelectedText)
			{
				MdiParent = this.parent
			}.Show();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "lol.launcher.exe|lol.launcher.exe";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog.FileName;
				this.textBox4.Text = fileName;
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
			this.button1 = new Button();
			this.button2 = new Button();
			this.label1 = new Label();
			this.textBox1 = new TextBox();
			this.textBox2 = new TextBox();
			this.label2 = new Label();
			this.button3 = new Button();
			this.label3 = new Label();
			this.numericUpDown1 = new NumericUpDown();
			this.label4 = new Label();
			this.numericUpDown2 = new NumericUpDown();
			this.label5 = new Label();
			this.comboBox1 = new ComboBox();
			this.label6 = new Label();
			this.textBox3 = new TextBox();
			this.label7 = new Label();
			this.comboBox2 = new ComboBox();
			this.comboBox3 = new ComboBox();
			this.groupBox1 = new GroupBox();
			this.comboBox4 = new ComboBox();
			this.label9 = new Label();
			this.textBox4 = new TextBox();
			this.label8 = new Label();
			this.button4 = new Button();
			this.checkBox2 = new CheckBox();
			this.checkBox1 = new CheckBox();
			((ISupportInitialize)this.numericUpDown1).BeginInit();
			((ISupportInitialize)this.numericUpDown2).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.button1.Location = new Point(277, 326);
			this.button1.Name = "button1";
			this.button1.Size = new Size(138, 34);
			this.button1.TabIndex = 0;
			this.button1.Text = "开始";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.button2.Location = new Point(196, 337);
			this.button2.Name = "button2";
			this.button2.Size = new Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "关闭";
			this.button2.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(1, 34);
			this.label1.Name = "label1";
			this.label1.Size = new Size(177, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "当前英雄联盟游戏版本号:";
			this.textBox1.Location = new Point(184, 31);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(208, 20);
			this.textBox1.TabIndex = 3;
			this.textBox2.Location = new Point(184, 83);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new Size(177, 20);
			this.textBox2.TabIndex = 4;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(85, 86);
			this.label2.Name = "label2";
			this.label2.Size = new Size(93, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "accounts.txt-路径:";
			this.button3.Location = new Point(367, 83);
			this.button3.Name = "button3";
			this.button3.Size = new Size(25, 20);
			this.button3.TabIndex = 6;
			this.button3.Text = "...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new EventHandler(this.button3_Click);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(113, 111);
			this.label3.Name = "label3";
			this.label3.Size = new Size(65, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "最大挂机数:";
			this.numericUpDown1.Location = new Point(184, 109);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new Size(208, 20);
			this.numericUpDown1.TabIndex = 8;
			NumericUpDown arg_4BC_0 = this.numericUpDown1;
			int[] array = new int[4];
			array[0] = 5;
			arg_4BC_0.Value = new decimal(array);
			this.label4.AutoSize = true;
			this.label4.Location = new Point(113, 137);
			this.label4.Name = "label4";
			this.label4.Size = new Size(69, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "挂到几等级:";
			this.numericUpDown2.Location = new Point(184, 135);
			this.numericUpDown2.Name = "numericUpDown2";
			this.numericUpDown2.Size = new Size(208, 20);
			this.numericUpDown2.TabIndex = 10;
			NumericUpDown arg_58B_0 = this.numericUpDown2;
			int[] array2 = new int[4];
			array2[0] = 10;
			arg_58B_0.Value = new decimal(array2);
			this.label5.AutoSize = true;
			this.label5.Location = new Point(134, 164);
			this.label5.Name = "label5";
			this.label5.Size = new Size(44, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "服务器:";
			this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new Point(184, 161);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new Size(208, 21);
			this.comboBox1.TabIndex = 12;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(100, 191);
			this.label6.Name = "label6";
			this.label6.Size = new Size(78, 13);
			this.label6.TabIndex = 13;
			this.label6.Text = "英雄选择:";
			this.textBox3.Location = new Point(184, 188);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new Size(208, 20);
			this.textBox3.TabIndex = 14;
			this.textBox3.Text = "Annie";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(87, 217);
			this.label7.Name = "label7";
			this.label7.Size = new Size(91, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "召唤师技能:";
			this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new Point(184, 214);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new Size(104, 21);
			this.comboBox2.TabIndex = 16;
			this.comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new Point(294, 214);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new Size(98, 21);
			this.comboBox3.TabIndex = 17;
			this.groupBox1.Controls.Add(this.comboBox4);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.textBox4);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.checkBox2);
			this.groupBox1.Controls.Add(this.checkBox1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.comboBox3);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.comboBox2);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBox3);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.numericUpDown1);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.numericUpDown2);
			this.groupBox1.Location = new Point(12, 26);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(403, 294);
			this.groupBox1.TabIndex = 18;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "选项(这里的选项和图形界面版本一样，新旧版功能也一样）";
			this.comboBox4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Location = new Point(184, 264);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new Size(208, 21);
			this.comboBox4.TabIndex = 24;
			this.label9.AutoSize = true;
			this.label9.Location = new Point(133, 267);
			this.label9.Name = "label9";
			this.label9.Size = new Size(45, 13);
			this.label9.TabIndex = 23;
			this.label9.Text = "挂机选项（BOT为人机）";
			this.textBox4.Location = new Point(184, 57);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new Size(177, 20);
			this.textBox4.TabIndex = 20;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(51, 61);
			this.label8.Name = "label8";
			this.label8.Size = new Size(127, 13);
			this.label8.TabIndex = 21;
			this.label8.Text = "英雄联盟游戏地址:";
			this.button4.Location = new Point(367, 57);
			this.button4.Name = "button4";
			this.button4.Size = new Size(25, 20);
			this.button4.TabIndex = 22;
			this.button4.Text = "...";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new EventHandler(this.button4_Click);
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new Point(294, 241);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new Size(102, 17);
			this.checkBox2.TabIndex = 19;
			this.checkBox2.Text = "是否小屏?";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(184, 241);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(98, 17);
			this.checkBox1.TabIndex = 18;
			this.checkBox1.Text = "随机召唤师技能?";
			this.checkBox1.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(427, 372);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			this.DoubleBuffered = true;
			base.Name = "old_volibot";
			this.Text = "（旧）VOLIBOT命令行版";
			base.Load += new EventHandler(this.old_volibot_Load);
			((ISupportInitialize)this.numericUpDown1).EndInit();
			((ISupportInitialize)this.numericUpDown2).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
