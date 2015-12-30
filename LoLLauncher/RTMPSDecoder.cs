using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace LoLLauncher
{
	public class RTMPSDecoder
	{
		private byte[] dataBuffer;

		private int dataPos;

		private List<string> stringReferences = new List<string>();

		private List<object> objectReferences = new List<object>();

		private List<ClassDefinition> classDefinitions = new List<ClassDefinition>();

		private void Reset()
		{
			this.stringReferences.Clear();
			this.objectReferences.Clear();
			this.classDefinitions.Clear();
		}

		public TypedObject DecodeConnect(byte[] data)
		{
			this.Reset();
			this.dataBuffer = data;
			this.dataPos = 0;
			TypedObject typedObject = new TypedObject("Invoke");
			typedObject.Add("result", this.DecodeAMF0());
			typedObject.Add("invokeId", this.DecodeAMF0());
			typedObject.Add("serviceCall", this.DecodeAMF0());
			typedObject.Add("data", this.DecodeAMF0());
			if (this.dataPos != this.dataBuffer.Length)
			{
				for (int i = this.dataPos; i < data.Length; i++)
				{
					if (this.ReadByte() != 0)
					{
						throw new Exception("There is other data in the buffer!");
					}
				}
			}
			if (this.dataPos != this.dataBuffer.Length)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Did not consume entire buffer: ",
					this.dataPos,
					" of ",
					this.dataBuffer.Length
				}));
			}
			return typedObject;
		}

		public TypedObject DecodeInvoke(byte[] data)
		{
			this.Reset();
			this.dataBuffer = data;
			this.dataPos = 0;
			TypedObject typedObject = new TypedObject("Invoke");
			if (this.dataBuffer[0] == 0)
			{
				this.dataPos++;
				typedObject.Add("version", 0);
			}
			typedObject.Add("result", this.DecodeAMF0());
			typedObject.Add("invokeId", this.DecodeAMF0());
			typedObject.Add("serviceCall", this.DecodeAMF0());
			typedObject.Add("data", this.DecodeAMF0());
			if (this.dataPos != this.dataBuffer.Length)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Did not consume entire buffer: ",
					this.dataPos,
					" of ",
					this.dataBuffer.Length
				}));
			}
			return typedObject;
		}

		public object Decode(byte[] data)
		{
			this.dataBuffer = data;
			this.dataPos = 0;
			object result = this.Decode();
			if (this.dataPos != this.dataBuffer.Length)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Did not consume entire buffer: ",
					this.dataPos,
					" of ",
					this.dataBuffer.Length
				}));
			}
			return result;
		}

		private object Decode()
		{
			byte b = this.ReadByte();
			switch (b)
			{
			case 0:
				throw new Exception("Undefined data type");
			case 1:
				return null;
			case 2:
				return false;
			case 3:
				return true;
			case 4:
				return this.ReadInt();
			case 5:
				return this.ReadDouble();
			case 6:
				return this.ReadString();
			case 7:
				return this.ReadXML();
			case 8:
				return this.ReadDate();
			case 9:
				return this.ReadArray();
			case 10:
				return this.ReadObject();
			case 11:
				return this.ReadXMLString();
			case 12:
				return this.ReadByteArray();
			default:
				throw new Exception("Unexpected AMF3 data type: " + b);
			}
		}

		private byte ReadByte()
		{
			byte result = this.dataBuffer[this.dataPos];
			this.dataPos++;
			return result;
		}

		private int ReadByteAsInt()
		{
			int num = (int)this.ReadByte();
			if (num < 0)
			{
				num += 256;
			}
			return num;
		}

		private byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = this.dataBuffer[this.dataPos];
				this.dataPos++;
			}
			return array;
		}

		private int ReadInt()
		{
			int num = this.ReadByteAsInt();
			if (num < 128)
			{
				return num;
			}
			num = (num & 127) << 7;
			int num2 = this.ReadByteAsInt();
			if (num2 < 128)
			{
				num |= num2;
			}
			else
			{
				num = (num | (num2 & 127)) << 7;
				num2 = this.ReadByteAsInt();
				if (num2 < 128)
				{
					num |= num2;
				}
				else
				{
					num = (num | (num2 & 127)) << 8;
					num2 = this.ReadByteAsInt();
					num |= num2;
				}
			}
			int num3 = 268435456;
			return -(num & num3) | num;
		}

		private double ReadDouble()
		{
			long num = 0L;
			for (int i = 0; i < 8; i++)
			{
				num = (num << 8) + (long)this.ReadByteAsInt();
			}
			return BitConverter.Int64BitsToDouble(num);
		}

		private string ReadString()
		{
			int num = this.ReadInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (!flag)
			{
				return this.stringReferences[num];
			}
			if (num == 0)
			{
				return "";
			}
			byte[] array = this.ReadBytes(num);
			string @string;
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				@string = uTF8Encoding.GetString(array);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Error parsing AMF3 string from ",
					array,
					'\n',
					ex.Message
				}));
			}
			this.stringReferences.Add(@string);
			return @string;
		}

		private string ReadXML()
		{
			throw new NotImplementedException("Reading of XML is not implemented");
		}

		private DateTime ReadDate()
		{
			int num = this.ReadInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (flag)
			{
				long num2 = (long)this.ReadDouble();
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				dateTime = dateTime.AddSeconds((double)(num2 / 1000L));
				this.objectReferences.Add(dateTime);
				return dateTime;
			}
			return DateTime.MinValue;
		}

		private object[] ReadArray()
		{
			int num = this.ReadInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (!flag)
			{
				return (object[])this.objectReferences[num];
			}
			string text = this.ReadString();
			if (text != null && !text.Equals(""))
			{
				throw new NotImplementedException("Associative arrays are not supported");
			}
			object[] array = new object[num];
			this.objectReferences.Add(array);
			for (int i = 0; i < num; i++)
			{
				array[i] = this.Decode();
			}
			return array;
		}

		private List<object> ReadList()
		{
			int num = this.ReadInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (!flag)
			{
				return (List<object>)this.objectReferences[num];
			}
			string text = this.ReadString();
			if (text != null && !text.Equals(""))
			{
				throw new NotImplementedException("Associative arrays are not supported");
			}
			List<object> list = new List<object>();
			this.objectReferences.Add(list);
			for (int i = 0; i < num; i++)
			{
				list.Add(this.Decode());
			}
			return list;
		}

		private object ReadObject()
		{
			int num = this.ReadInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (flag)
			{
				bool flag2 = (num & 1) != 0;
				num >>= 1;
				ClassDefinition classDefinition;
				if (flag2)
				{
					classDefinition = new ClassDefinition();
					classDefinition.type = this.ReadString();
					classDefinition.externalizable = ((num & 1) != 0);
					num >>= 1;
					classDefinition.dynamic = ((num & 1) != 0);
					num >>= 1;
					for (int i = 0; i < num; i++)
					{
						classDefinition.members.Add(this.ReadString());
					}
					this.classDefinitions.Add(classDefinition);
				}
				else
				{
					classDefinition = this.classDefinitions[num];
				}
				TypedObject typedObject = new TypedObject(classDefinition.type);
				this.objectReferences.Add(typedObject);
				if (classDefinition.externalizable)
				{
					if (classDefinition.type.Equals("DSK"))
					{
						typedObject = this.ReadDSK();
					}
					else if (classDefinition.type.Equals("DSA"))
					{
						typedObject = this.ReadDSA();
					}
					else if (classDefinition.type.Equals("flex.messaging.io.ArrayCollection"))
					{
						object obj = this.Decode();
						typedObject = TypedObject.MakeArrayCollection((object[])obj);
					}
					else
					{
						if (!classDefinition.type.Equals("com.riotgames.platform.systemstate.ClientSystemStatesNotification") && !classDefinition.type.Equals("com.riotgames.platform.broadcast.BroadcastNotification"))
						{
							throw new NotImplementedException("Externalizable not handled for " + classDefinition.type);
						}
						int num2 = 0;
						for (int j = 0; j < 4; j++)
						{
							num2 = num2 * 256 + this.ReadByteAsInt();
						}
						byte[] array = this.ReadBytes(num2);
						StringBuilder stringBuilder = new StringBuilder();
						for (int k = 0; k < array.Length; k++)
						{
							stringBuilder.Append(Convert.ToChar(array[k]));
						}
						JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
						typedObject = javaScriptSerializer.Deserialize<TypedObject>(stringBuilder.ToString());
						typedObject.type = classDefinition.type;
					}
				}
				else
				{
					for (int l = 0; l < classDefinition.members.Count; l++)
					{
						string key = classDefinition.members[l];
						object value = this.Decode();
						typedObject.Add(key, value);
					}
					if (classDefinition.dynamic)
					{
						string key2;
						while ((key2 = this.ReadString()).Length != 0)
						{
							object value2 = this.Decode();
							typedObject.Add(key2, value2);
						}
					}
				}
				return typedObject;
			}
			return this.objectReferences[num];
		}

		private string ReadXMLString()
		{
			throw new NotImplementedException("Reading of XML strings is not implemented");
		}

		private byte[] ReadByteArray()
		{
			int num = this.ReadInt();
			bool flag = (num & 1) != 0;
			num >>= 1;
			if (flag)
			{
				byte[] array = this.ReadBytes(num);
				this.objectReferences.Add(array);
				return array;
			}
			return (byte[])this.objectReferences[num];
		}

		private TypedObject ReadDSA()
		{
			TypedObject typedObject = new TypedObject("DSA");
			List<int> list = this.ReadFlags();
			for (int i = 0; i < list.Count; i++)
			{
				int num = list[i];
				int bits = 0;
				if (i == 0)
				{
					if ((num & 1) != 0)
					{
						typedObject.Add("body", this.Decode());
					}
					if ((num & 2) != 0)
					{
						typedObject.Add("clientId", this.Decode());
					}
					if ((num & 4) != 0)
					{
						typedObject.Add("destination", this.Decode());
					}
					if ((num & 8) != 0)
					{
						typedObject.Add("headers", this.Decode());
					}
					if ((num & 16) != 0)
					{
						typedObject.Add("messageId", this.Decode());
					}
					if ((num & 32) != 0)
					{
						typedObject.Add("timeStamp", this.Decode());
					}
					if ((num & 64) != 0)
					{
						typedObject.Add("timeToLive", this.Decode());
					}
					bits = 7;
				}
				else if (i == 1)
				{
					if ((num & 1) != 0)
					{
						this.ReadByte();
						byte[] array = this.ReadByteArray();
						typedObject.Add("clientIdBytes", array);
						typedObject.Add("clientId", this.ByteArrayToID(array));
					}
					if ((num & 2) != 0)
					{
						this.ReadByte();
						byte[] array2 = this.ReadByteArray();
						typedObject.Add("messageIdBytes", array2);
						typedObject.Add("messageId", this.ByteArrayToID(array2));
					}
					bits = 2;
				}
				this.ReadRemaining(num, bits);
			}
			list = this.ReadFlags();
			for (int j = 0; j < list.Count; j++)
			{
				int num = list[j];
				int bits2 = 0;
				if (j == 0)
				{
					if ((num & 1) != 0)
					{
						typedObject.Add("correlationId", this.Decode());
					}
					if ((num & 2) != 0)
					{
						this.ReadByte();
						byte[] array3 = this.ReadByteArray();
						typedObject.Add("correlationIdBytes", array3);
						typedObject.Add("correlationId", this.ByteArrayToID(array3));
					}
					bits2 = 2;
				}
				this.ReadRemaining(num, bits2);
			}
			return typedObject;
		}

		private TypedObject ReadDSK()
		{
			TypedObject typedObject = this.ReadDSA();
			typedObject.type = "DSK";
			List<int> list = this.ReadFlags();
			for (int i = 0; i < list.Count; i++)
			{
				this.ReadRemaining(list[i], 0);
			}
			return typedObject;
		}

		private List<int> ReadFlags()
		{
			List<int> list = new List<int>();
			int num;
			do
			{
				num = this.ReadByteAsInt();
				list.Add(num);
			}
			while ((num & 128) != 0);
			return list;
		}

		private void ReadRemaining(int flag, int bits)
		{
			if (flag >> bits != 0)
			{
				for (int i = bits; i < 6; i++)
				{
					if ((flag >> i & 1) != 0)
					{
						this.Decode();
					}
				}
			}
		}

		private string ByteArrayToID(byte[] data)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				if (i == 4 || i == 6 || i == 8 || i == 10)
				{
					stringBuilder.Append('-');
				}
				stringBuilder.AppendFormat("{0:X2}", data[i]);
			}
			return stringBuilder.ToString();
		}

		private object DecodeAMF0()
		{
			int num = (int)this.ReadByte();
			int num2 = num;
			switch (num2)
			{
			case 0:
				return this.ReadIntAMF0();
			case 1:
			case 4:
				break;
			case 2:
				return this.ReadStringAMF0();
			case 3:
				return this.ReadObjectAMF0();
			case 5:
				return null;
			default:
				if (num2 == 17)
				{
					return this.Decode();
				}
				break;
			}
			throw new NotImplementedException("AMF0 type not supported: " + num);
		}

		private string ReadStringAMF0()
		{
			int num = (this.ReadByteAsInt() << 8) + this.ReadByteAsInt();
			if (num == 0)
			{
				return "";
			}
			byte[] array = this.ReadBytes(num);
			string @string;
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				@string = uTF8Encoding.GetString(array);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Error parsing AMF0 string from ",
					array,
					'\n',
					ex.Message
				}));
			}
			return @string;
		}

		private int ReadIntAMF0()
		{
			return (int)this.ReadDouble();
		}

		private TypedObject ReadObjectAMF0()
		{
			TypedObject typedObject = new TypedObject("Body");
			string key;
			while (!(key = this.ReadStringAMF0()).Equals(""))
			{
				byte b = this.ReadByte();
				if (b == 0)
				{
					typedObject.Add(key, this.ReadDouble());
				}
				else if (b == 2)
				{
					typedObject.Add(key, this.ReadStringAMF0());
				}
				else
				{
					if (b != 5)
					{
						throw new NotImplementedException("AMF0 type not supported: " + b);
					}
					typedObject.Add(key, null);
				}
			}
			this.ReadByte();
			return typedObject;
		}
	}
}
