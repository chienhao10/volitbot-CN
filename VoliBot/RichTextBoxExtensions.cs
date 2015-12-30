using System;
using System.Drawing;
using System.Windows.Forms;

namespace VoliBot
{
	public static class RichTextBoxExtensions
	{
		public static void AppendText(this RichTextBox box, string text, Color color)
		{
			try
			{
				if (box.InvokeRequired)
				{
					box.Invoke(new Action(delegate
					{
						box.AppendText(text, color);
					}));
				}
				else
				{
					box.SelectionStart = box.TextLength;
					box.SelectionLength = 0;
					box.SelectionColor = color;
					box.AppendText(text);
					box.SelectionColor = box.ForeColor;
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
