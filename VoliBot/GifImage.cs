using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace VoliBot
{
	public class GifImage
	{
		private Image gifImage;

		private FrameDimension dimension;

		private int frameCount;

		private int currentFrame = -1;

		private bool reverse;

		private int step = 1;

		public bool ReverseAtEnd
		{
			get
			{
				return this.reverse;
			}
			set
			{
				this.reverse = value;
			}
		}

		public GifImage(Image path)
		{
			this.gifImage = path;
			this.dimension = new FrameDimension(this.gifImage.FrameDimensionsList[0]);
			this.frameCount = this.gifImage.GetFrameCount(this.dimension);
		}

		public Image GetNextFrame()
		{
			this.currentFrame += this.step;
			if (this.currentFrame >= this.frameCount || this.currentFrame < 1)
			{
				if (this.reverse)
				{
					this.step *= -1;
					this.currentFrame += this.step;
				}
				else
				{
					this.currentFrame = 0;
				}
			}
			return this.GetFrame(this.currentFrame);
		}

		public Image GetFrame(int index)
		{
			this.gifImage.SelectActiveFrame(this.dimension, index);
			return (Image)this.gifImage.Clone();
		}
	}
}
