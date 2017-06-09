using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Proto
{
    public class ProtocolManager
    {
        private Dictionary<int, Func<byte, byte, byte[], MessageObject>> m_ReceiveProtocolMapping = new Dictionary<int, Func<byte, byte, byte[], MessageObject>>();
        private Dictionary<int, Func<byte, byte, MessageObject, byte[]>> m_SendProtocolMapping = new Dictionary<int, Func<byte, byte, MessageObject, byte[]>>();

        public static void AddReceiveProtocol(byte cmdType, byte cmdID)
        {

        }

        public static void AddReceiveProtocol<T>(byte cmdType, byte cmdID) where T: IExtensible
        {

        }

        public static void AddSendProtocol(byte cmdType, byte cmdID)
        {

        }

        public static void AddSendProtocol<T>(byte cmdType, byte cmdID) where T:IExtensible
        {

        }

        public static MessageObject Deserialize<T>(byte cmdType, byte cmdID, byte[] buff) where T:IExtensible
        {
            return null;
        }

        public static byte[] Serialize(MessageObject obj)
        {
            return null;
        }

        public static byte[] Serialize<T>(MessageObject obj) where T:IExtensible
        {
            return null;
        }

        public static MessageObject GetMessageObject(byte[] buff)
        {
            return null;
        }

        public static byte[] GetBuff(MessageObject obj)
        {
            return null;
        }
        private int GetUniqueID(byte cmdType, byte cmdID)
        {
            return -1;
        }
    }
}
