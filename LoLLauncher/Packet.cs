using System;
using System.Collections.Generic;

namespace LoLLauncher
{
	public class Packet
	{
		private byte[] dataBuffer;

		private int dataPos;

		private int dataSize;

		private int packetType;

		private List<byte> rawPacketBytes;

		public Packet()
		{
			this.rawPacketBytes = new List<byte>();
		}

		public void SetSize(int size)
		{
			this.dataSize = size;
			this.dataBuffer = new byte[this.dataSize];
		}

		public void SetType(int type)
		{
			this.packetType = type;
		}

		public void Add(byte b)
		{
			this.dataBuffer[this.dataPos++] = b;
		}

		public bool IsComplete()
		{
			return this.dataPos == this.dataSize;
		}

		public int GetSize()
		{
			return this.dataSize;
		}

		public int GetPacketType()
		{
			return this.packetType;
		}

		public byte[] GetData()
		{
			return this.dataBuffer;
		}

		public void AddToRaw(byte b)
		{
			this.rawPacketBytes.Add(b);
		}

		public void AddToRaw(byte[] b)
		{
			this.rawPacketBytes.AddRange(b);
		}

		public byte[] GetRawData()
		{
			return this.rawPacketBytes.ToArray();
		}
	}
}
