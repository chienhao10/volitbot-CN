using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VoliBots
{
	public class exListBox : ListBox
	{
		private Size _imageSize;

		private StringFormat _fmt;

		private Font _titleFont;

		private Font _detailsFont;

		private Font _levelFont;

		private IContainer components;

		public exListBox(Font titleFont, Font detailsFont, Font levelFont, Size imageSize, StringAlignment aligment, StringAlignment lineAligment)
		{
			this._titleFont = titleFont;
			this._detailsFont = detailsFont;
			this._levelFont = levelFont;
			this._imageSize = imageSize;
			this.ItemHeight = this._imageSize.Height + base.Margin.Vertical;
			this._fmt = new StringFormat();
			this._fmt.Alignment = aligment;
			this._fmt.LineAlignment = lineAligment;
			this._titleFont = titleFont;
			this._detailsFont = detailsFont;
			this._levelFont = levelFont;
		}

		public exListBox()
		{
			this.InitializeComponent();
			this._imageSize = new Size(50, 50);
			this.ItemHeight = this._imageSize.Height + base.Margin.Vertical;
			this._fmt = new StringFormat();
			this._fmt.Alignment = StringAlignment.Near;
			this._fmt.LineAlignment = StringAlignment.Near;
			this._titleFont = new Font(this.Font, FontStyle.Bold);
			this._detailsFont = new Font(this.Font, FontStyle.Regular);
			this._levelFont = new Font(this.Font, FontStyle.Regular);
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (base.Items.Count > 0)
			{
				exListBoxItem exListBoxItem = (exListBoxItem)base.Items[e.Index];
				exListBoxItem.drawItem(e, base.Margin, this._titleFont, this._detailsFont, this._levelFont, this._fmt, this._imageSize);
			}
		}

		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			e.ItemHeight = 51;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
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
		}
	}
}
