using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Proto
{
    public class ProtocolManager
    {
        private static Dictionary<int, Func<byte, byte, byte[], MessageObject>> m_ReceiveProtocolMapping = new Dictionary<int, Func<byte, byte, byte[], MessageObject>>();
		//private static Dictionary<int, Func<byte, byte, MessageObject, byte[]>> m_SendProtocolMapping = new Dictionary<int, Func<byte, byte, MessageObject, byte[]>>();
		private static Dictionary<int, Func<MessageObject, byte[]>> m_SendProtocolMapping = new Dictionary<int, Func<MessageObject, byte[]>>();

        public static void AddReceiveProtocol(byte cmdType, byte cmdID)
        {
			int uniqueID = GetUniqueID(cmdType, cmdID);
			if (m_ReceiveProtocolMapping.ContainsKey(uniqueID))
				m_ReceiveProtocolMapping.Remove(uniqueID);
			m_ReceiveProtocolMapping.Add(uniqueID, null);
        }

        public static void AddReceiveProtocol<T>(byte cmdType, byte cmdID) where T: IExtensible
        {
			int uniqueID = GetUniqueID(cmdType, cmdID);
			if (m_ReceiveProtocolMapping.ContainsKey(uniqueID))
				m_ReceiveProtocolMapping.Remove(uniqueID);
			m_ReceiveProtocolMapping.Add(uniqueID, Deserialize<T>);
        }

        public static void AddSendProtocol(byte cmdType, byte cmdID)
        {
			int uniqueID = GetUniqueID(cmdType, cmdID);
			if (m_SendProtocolMapping.ContainsKey(uniqueID))
				m_SendProtocolMapping.Remove(uniqueID);
			m_SendProtocolMapping.Add(uniqueID, Serialize);
        }

        public static void AddSendProtocol<T>(byte cmdType, byte cmdID) where T:IExtensible
        {
			int uniqueID = GetUniqueID(cmdType, cmdID);
			if (m_SendProtocolMapping.ContainsKey(uniqueID))
				m_SendProtocolMapping.Remove(uniqueID);
			m_SendProtocolMapping.Add(uniqueID, Serialize<T>);
        }

        public static MessageObject Deserialize<T>(byte cmdType, byte cmdID, byte[] buff) where T:IExtensible
        {
			//TODO: 这里需要修改成只传入byte[]来获取MessageObject
			T data = default(T);
			data = SerializeUtil.DeSerializeProto<T>(buff);
            return MessageObject.GetMessageObject(cmdType, cmdID, data);
        }

        public static byte[] Serialize(MessageObject obj)
        {
            return null;
        }

        public static byte[] Serialize<T>(MessageObject obj) where T:IExtensible
        {
            return SerializeUtil.SerializeProto<T>(obj);
        }

        //public static MessageObject GetMessageObject(byte[] buff)
        //{
        //    return null;
        //}

        //public static byte[] GetBuff<T>(MessageObject obj)
        //{
        //    return SerializeUtil.SerializeProto<T>(obj); ;
        //}
        private static int GetUniqueID(byte cmdType, byte cmdID)
        {
            return cmdType * 0xff + cmdID;
        }
    }
}
