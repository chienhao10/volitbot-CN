using System;
using System.Drawing;
using System.Windows.Forms;

namespace VoliBots
{
	internal class exListBoxItem
	{
		private string _title;

		private string _details;

		private string _level;

		private Image _itemImage;

		private string _id;

		public string Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
			}
		}

		public string Details
		{
			get
			{
				return this._details;
			}
			set
			{
				this._details = value;
			}
		}

		public string Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
			}
		}

		public Image ItemImage
		{
			get
			{
				return this._itemImage;
			}
			set
			{
				this._itemImage = value;
			}
		}

		public exListBoxItem(string id, string title, string details, string level, Image image)
		{
			this._id = id;
			this._title = title;
			this._details = details;
			this._level = level;
			this._itemImage = image;
		}

		public void drawItem(DrawItemEventArgs e, Padding margin, Font titleFont, Font detailsFont, Font levelFont, StringFormat aligment, Size imageSize)
		{
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 63, 147, 236)), e.Bounds);
			}
			else
			{
				e.Graphics.FillRectangle(Brushes.WhiteSmoke, e.Bounds);
			}
			if (e.Index == 0)
			{
				e.Graphics.DrawLine(Pens.Gray, e.Bounds.X, e.Bounds.Y, e.Bounds.X + e.Bounds.Width, e.Bounds.Y);
			}
			e.Graphics.DrawLine(Pens.Gray, e.Bounds.X, e.Bounds.Y + 50, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + 50);
			e.Graphics.DrawImage(this.ItemImage, e.Bounds.X + margin.Left, e.Bounds.Y + margin.Top, imageSize.Width, imageSize.Height);
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(170, 0, 0, 0)), e.Bounds.X + margin.Left, e.Bounds.Y + 27 + margin.Top, imageSize.Width, 23);
			Rectangle r = new Rectangle(e.Bounds.X + margin.Horizontal + imageSize.Width + 3, e.Bounds.Y + margin.Top + 5, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal, (int)titleFont.GetHeight() + 2);
			Rectangle r2 = new Rectangle(e.Bounds.X + margin.Horizontal + imageSize.Width + 5, e.Bounds.Y + (int)titleFont.GetHeight() + 10 + margin.Vertical + margin.Top, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal, e.Bounds.Height - margin.Bottom - (int)titleFont.GetHeight() - 15 - margin.Vertical - margin.Top);
			Rectangle r3 = new Rectangle(e.Bounds.X, e.Bounds.Y + 27, imageSize.Width, 23);
			e.Graphics.DrawLine(Pens.Black, e.Bounds.X + margin.Horizontal + imageSize.Width + 6, e.Bounds.Y + (int)titleFont.GetHeight() + margin.Vertical + r2.Height + margin.Top, e.Bounds.Width - margin.Right - imageSize.Width - margin.Horizontal + 33, e.Bounds.Y + (int)titleFont.GetHeight() + margin.Vertical + margin.Top + r2.Height);
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;
			if (this.Level != "")
			{
				e.Graphics.DrawString(this.Level, titleFont, Brushes.White, r3, stringFormat);
			}
			else
			{
				e.Graphics.DrawString("?", titleFont, Brushes.White, r3, stringFormat);
			}
			e.Graphics.DrawString(this.Title, titleFont, Brushes.Black, r, aligment);
			e.Graphics.DrawString(this.Details, detailsFont, Brushes.Black, r2, aligment);
			e.DrawFocusRectangle();
		}
	}
}
