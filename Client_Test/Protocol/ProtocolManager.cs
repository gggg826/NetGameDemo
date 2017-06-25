using System;
using System.IO;
using System.Collections.Generic;
using ProtoBuf;

namespace Protocol
{
    public class ProtocolManager
    {
        private static Dictionary<int, Func<Package_Head, byte[], MessageObject>> m_ReceiveProtocolMapping = new Dictionary<int, Func<Package_Head, byte[], MessageObject>>();
		private static Dictionary<int, Func<MessageObject, byte[]>> m_SendProtocolMapping = new Dictionary<int, Func<MessageObject, byte[]>>();


        public static MessageObject GetMessageObjectFromBuff(byte[] buff)
        {
            if (m_ReceiveProtocolMapping.Count == 0)
            {
                Console.WriteLine("获取MessageObject出错，接收协议池为空");
                return null;
            }

            int headBuffLength = ProtoStructDefine.Package_head_Length;
            if (buff == null || buff.Length < headBuffLength)
                return null;

            byte[] headBuff = new byte[headBuffLength];
            Buffer.BlockCopy(buff, 0, headBuff, 0, headBuffLength);
            Package_Head head = (Package_Head)ByteUtil.BytesToStuct(headBuff, typeof(Package_Head), headBuffLength);

            int uniqueID = GetUniqueID(head.CmdType, head.CmdID);
            if (m_ReceiveProtocolMapping.ContainsKey(uniqueID))
            {
                byte[] messageBuff = null;
                int messageBuffLength = buff.Length - headBuffLength;
                if (messageBuffLength != 0)
                {
                    messageBuff = new byte[messageBuffLength];
                    Buffer.BlockCopy(buff, headBuffLength, messageBuff, 0, messageBuffLength);
                }

                Func<Package_Head, byte[], MessageObject> func = m_ReceiveProtocolMapping[uniqueID];
                return func(head, messageBuff);
            }
            return new MessageObject(head.CmdType, head.CmdID, null);

        }

        public static byte[] GetBuffFromMessageObject<T>(MessageObject obj)
        {
            if (obj == null || m_SendProtocolMapping.Count == 0)
            {
                Console.WriteLine("获取byte[]出错，发送协议池为空");
                return null;
            }

            int uniqueID = GetUniqueID(obj.CmdType, obj.CmdID);
            if (m_SendProtocolMapping.ContainsKey(uniqueID))
            {
                Func<MessageObject, byte[]> func = m_SendProtocolMapping[uniqueID];
                return func(obj);
            }
            return null;
        }

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

        private static MessageObject Deserialize<T>(Package_Head head, byte[] msgBuff) where T:IExtensible
        {
            int messageBuffLength = msgBuff.Length;
            object message = null;
            if (messageBuffLength != 0)
            {
                message = SerializeUtil.DeSerializeProto<T>(msgBuff);
            }
			
            return MessageObject.GetMessageObject(head.CmdType, head.CmdID, message);
        }

        private static byte[] Serialize(MessageObject obj)
        {
            return null;
        }

        private static byte[] Serialize<T>(MessageObject obj) where T:IExtensible
        {
            int headBuffLength = ProtoStructDefine.Package_head_Length;
            Package_Head head = new Package_Head(obj.CmdType, obj.CmdID);
            byte[] headBuff = ByteUtil.StructToBytes(head, headBuffLength);

            byte[] messageBuff = null;
            if (obj.Message != null && obj.Message is T)
                messageBuff = SerializeUtil.SerializeProto<T>(obj.Message);
            int messageBuffLength = messageBuff == null? 0 : messageBuff.Length;

            byte[] buff = new byte[headBuffLength + messageBuffLength];

            Array.Copy(headBuff, 0, buff, 0, headBuffLength);
            if(messageBuff != null)
                Array.Copy(messageBuff, 0, buff, headBuffLength, messageBuffLength);
            return buff;
        }

        private static int GetUniqueID(byte cmdType, byte cmdID)
        {
            return cmdType * 0xff + cmdID;
        }
    }
}
