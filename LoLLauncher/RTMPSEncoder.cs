using System;
using System.Collections.Generic;
using System.Text;

namespace LoLLauncher
{
	public class RTMPSEncoder
	{
		public long startTime = (long)DateTime.Now.TimeOfDay.TotalMilliseconds;

		public byte[] AddHeaders(byte[] data)
		{
			List<byte> list = new List<byte>();
			list.Add(3);
			long num = (long)DateTime.Now.TimeOfDay.TotalMilliseconds - this.startTime;
			list.Add((byte)((num & 16711680L) >> 16));
			list.Add((byte)((num & 65280L) >> 8));
			list.Add((byte)(num & 255L));
			list.Add((byte)((data.Length & 16711680) >> 16));
			list.Add((byte)((data.Length & 65280) >> 8));
			list.Add((byte)(data.Length & 255));
			list.Add(17);
			list.Add(0);
			list.Add(0);
			list.Add(0);
			list.Add(0);
			for (int i = 0; i < data.Length; i++)
			{
				list.Add(data[i]);
				if (i % 128 == 127 && i != data.Length - 1)
				{
					list.Add(195);
				}
			}
			byte[] array = new byte[list.Count];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = list[j];
			}
			return array;
		}

		public byte[] EncodeConnect(Dictionary<string, object> paramaters)
		{
			List<byte> list = new List<byte>();
			this.WriteStringAMF0(list, "connect");
			this.WriteIntAMF0(list, 1);
			list.Add(17);
			list.Add(9);
			this.WriteAssociativeArray(list, paramaters);
			list.Add(1);
			list.Add(0);
			this.WriteStringAMF0(list, "nil");
			this.WriteStringAMF0(list, "");
			TypedObject typedObject = new TypedObject("flex.messaging.messages.CommandMessage");
			typedObject.Add("operation", 5);
			typedObject.Add("correlationId", "");
			typedObject.Add("timestamp", 0);
			typedObject.Add("messageId", RTMPSEncoder.RandomUID());
			typedObject.Add("body", new TypedObject(null));
			typedObject.Add("destination", "");
			typedObject.Add("headers", new Dictionary<string, object>
			{
				{
					"DSMessagingVersion",
					1.0
				},
				{
					"DSId",
					"my-rtmps"
				}
			});
			typedObject.Add("clientId", null);
			typedObject.Add("timeToLive", 0);
			list.Add(17);
			this.Encode(list, typedObject);
			byte[] array = new byte[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i];
			}
			array = this.AddHeaders(array);
			array[7] = 20;
			return array;
		}

		public byte[] EncodeInvoke(int id, object data)
		{
			List<byte> list = new List<byte>();
			list.Add(0);
			list.Add(5);
			this.WriteIntAMF0(list, id);
			list.Add(5);
			list.Add(17);
			this.Encode(list, data);
			byte[] array = new byte[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i];
			}
			return this.AddHeaders(array);
		}

		public byte[] Encode(object obj)
		{
			List<byte> list = new List<byte>();
			this.Encode(list, obj);
			byte[] array = new byte[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = list[i];
			}
			return array;
		}

		public void Encode(List<byte> ret, object obj)
		{
			if (obj == null)
			{
				ret.Add(1);
				return;
			}
			if (obj is bool)
			{
				bool flag = (bool)obj;
				if (flag)
				{
					ret.Add(3);
					return;
				}
				ret.Add(2);
				return;
			}
			else
			{
				if (obj is int)
				{
					ret.Add(4);
					this.WriteInt(ret, (int)obj);
					return;
				}
				if (obj is double)
				{
					ret.Add(5);
					this.WriteDouble(ret, (double)obj);
					return;
				}
				if (obj is string)
				{
					ret.Add(6);
					this.WriteString(ret, (string)obj);
					return;
				}
				if (obj is DateTime)
				{
					ret.Add(8);
					this.WriteDate(ret, (DateTime)obj);
					return;
				}
				if (obj is byte[])
				{
					ret.Add(12);
					this.WriteByteArray(ret, (byte[])obj);
					return;
				}
				if (obj is object[])
				{
					ret.Add(9);
					this.WriteArray(ret, (object[])obj);
					return;
				}
				if (obj is TypedObject)
				{
					ret.Add(10);
					this.WriteObject(ret, (TypedObject)obj);
					return;
				}
				if (obj is Dictionary<string, object>)
				{
					ret.Add(9);
					this.WriteAssociativeArray(ret, (Dictionary<string, object>)obj);
					return;
				}
				throw new Exception("Unexpected object type: " + obj.GetType().FullName);
			}
		}

		private void WriteInt(List<byte> ret, int val)
		{
			if (val < 0 || val >= 2097152)
			{
				ret.Add((byte)((val >> 22 & 127) | 128));
				ret.Add((byte)((val >> 15 & 127) | 128));
				ret.Add((byte)((val >> 8 & 127) | 128));
				ret.Add((byte)(val & 255));
				return;
			}
			if (val >= 16384)
			{
				ret.Add((byte)((val >> 14 & 127) | 128));
			}
			if (val >= 128)
			{
				ret.Add((byte)((val >> 7 & 127) | 128));
			}
			ret.Add((byte)(val & 127));
		}

		private void WriteDouble(List<byte> ret, double val)
		{
			if (double.IsNaN(val))
			{
				ret.Add(127);
				ret.Add(255);
				ret.Add(255);
				ret.Add(255);
				ret.Add(224);
				ret.Add(0);
				ret.Add(0);
				ret.Add(0);
				return;
			}
			byte[] bytes = BitConverter.GetBytes(val);
			for (int i = bytes.Length - 1; i >= 0; i--)
			{
				ret.Add(bytes[i]);
			}
		}

		private void WriteString(List<byte> ret, string val)
		{
			byte[] array = null;
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				array = uTF8Encoding.GetBytes(val);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Unable to encode string as UTF-8: ",
					val,
					'\n',
					ex.Message
				}));
			}
			this.WriteInt(ret, array.Length << 1 | 1);
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte item = array2[i];
				ret.Add(item);
			}
		}

		private void WriteDate(List<byte> ret, DateTime val)
		{
			ret.Add(1);
			this.WriteDouble(ret, val.TimeOfDay.TotalMilliseconds);
		}

		private void WriteArray(List<byte> ret, object[] val)
		{
			this.WriteInt(ret, val.Length << 1 | 1);
			ret.Add(1);
			for (int i = 0; i < val.Length; i++)
			{
				object obj = val[i];
				this.Encode(ret, obj);
			}
		}

		private void WriteAssociativeArray(List<byte> ret, Dictionary<string, object> val)
		{
			ret.Add(1);
			foreach (string current in val.Keys)
			{
				this.WriteString(ret, current);
				this.Encode(ret, val[current]);
			}
			ret.Add(1);
		}

		private void WriteObject(List<byte> ret, TypedObject val)
		{
			if (val.type == null || val.type.Equals(""))
			{
				ret.Add(11);
				ret.Add(1);
				foreach (string current in val.Keys)
				{
					this.WriteString(ret, current);
					this.Encode(ret, val[current]);
				}
				ret.Add(1);
				return;
			}
			if (val.type.Equals("flex.messaging.io.ArrayCollection"))
			{
				ret.Add(7);
				this.WriteString(ret, val.type);
				this.Encode(ret, val["array"]);
				return;
			}
			this.WriteInt(ret, val.Count << 4 | 3);
			this.WriteString(ret, val.type);
			List<string> list = new List<string>();
			foreach (string current2 in val.Keys)
			{
				this.WriteString(ret, current2);
				list.Add(current2);
			}
			foreach (string current3 in list)
			{
				this.Encode(ret, val[current3]);
			}
		}

		private void WriteByteArray(List<byte> ret, byte[] val)
		{
			throw new NotImplementedException("Encoding byte arrays is not implemented");
		}

		private void WriteIntAMF0(List<byte> ret, int val)
		{
			ret.Add(0);
			byte[] bytes = BitConverter.GetBytes((double)val);
			for (int i = bytes.Length - 1; i >= 0; i--)
			{
				ret.Add(bytes[i]);
			}
		}

		private void WriteStringAMF0(List<byte> ret, string val)
		{
			byte[] array = null;
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				array = uTF8Encoding.GetBytes(val);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Unable to encode string as UTF-8: ",
					val,
					'\n',
					ex.Message
				}));
			}
			ret.Add(2);
			ret.Add((byte)((array.Length & 65280) >> 8));
			ret.Add((byte)(array.Length & 255));
			byte[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				byte item = array2[i];
				ret.Add(item);
			}
		}

		public static string RandomUID()
		{
			Random random = new Random();
			byte[] array = new byte[16];
			random.NextBytes(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (i == 4 || i == 6 || i == 8 || i == 10)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.AppendFormat("{0:X2}", array[i]);
			}
			return stringBuilder.ToString();
		}
	}
}
