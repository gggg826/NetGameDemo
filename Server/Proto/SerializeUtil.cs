using System;
using System.IO;

namespace Proto
{
	public class SerializeUtil
	{
		private static PROTO m_Proto;

		private static PROTO GetPROTOObject()
		{
			if (m_Proto == null)
				m_Proto = new PROTO();
			return m_Proto;
		}

		public static byte[] SerializeProto<T>(MessageObject messageObj)
		{
			byte[] typeBuff = BitConverter.GetBytes(messageObj.CmdType);
			byte[] idBuff = BitConverter.GetBytes(messageObj.CmdID);
            
			byte[] messageBuff = null;
			using (MemoryStream ms = new MemoryStream())
			{
				PROTO proto = GetPROTOObject();
				if(proto.IsDefined(typeof(T)))
				{
					proto.Serialize(ms, messageObj.Message);
					messageBuff = ms.GetBuffer();
					messageBuff = ms.ToArray();
				}
			}

			int typeLength = typeBuff.Length;
			int idLength = idBuff.Length;
			int messageLength = messageBuff.Length;

			byte[] buff = new byte[typeLength + idLength + messageLength];
			Buffer.BlockCopy(typeBuff, 0, buff, 0, typeLength);
			Buffer.BlockCopy(idBuff, 0, buff, typeLength, idLength);
			Buffer.BlockCopy(messageBuff, 0, buff, typeLength + idLength, messageLength);

			return buff;
		}

		public static T DeSerializeProto<T>(byte[] buff)
		{
			if (buff == null)
				return default(T);
			object obj = null;

			try
			{
				using (MemoryStream ms = new MemoryStream(buff))
				{
					Type type = typeof(T);
					PROTO proto = GetPROTOObject();
					if(proto.IsDefined(type))
					{
						obj = proto.Deserialize(ms, obj, type);
					}
					else
					{
						return default(T);
					}
				}
			}
			catch (Exception e)
			{
				return default(T);
			}

			return (T)obj;
		}
	}
}
