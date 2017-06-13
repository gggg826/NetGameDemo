/********************************************************************
*
*	file base:	MessageObject
*	
*	purpose:	MessageObject数据结构和对象池，数据结构包含传输协议和消息体
*				用于序列化之前和反序列化之后
*	
*	created:	BingLau 
				13/6/2017   11:47
*********************************************************************/

using System;
using System.Collections.Generic;

namespace Protocol
{
    public class MessageObject
    {
        private byte m_CmdType;
        public byte CmdType
        {
            get
            {
                return m_CmdType;
            }

            set
            {
                m_CmdType = value;
            }
        }

        private byte m_CmdID;
        public byte CmdID
        {
            get
            {
                return m_CmdID;
            }

            set
            {
                m_CmdID = value;
            }
        }

        private object m_Message;
        public object Message
        {
            get
            {
                return m_Message;
            }

            set
            {
                m_Message = value;
            }
        }

        public MessageObject(byte cmdType, byte cmdID, object message)
        {
            m_CmdType = cmdType;
            m_CmdID = cmdID;
            m_Message = message;
        }

        /// <summary>
        /// 从对象池中取出一个MessageObject进行赋值复用
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static MessageObject GetMessageObject(byte cmdType, byte cmdID, object message)
        {
            MessageObject obj = GetCachedMessageObject();
            obj.CmdType = cmdType;
            obj.CmdID = cmdID;
            obj.Message = message;
            return obj;
        }

        /// <summary>
        /// 把MessageObject放回对象池
        /// </summary>
        /// <param name="message"></param>
        public static void ReleaseObject(MessageObject message)
        {
            if (message != null)
            {
                message.CmdType = 0;
                message.CmdID = 0;
                message.Message = null;
                RecycleMessageObject(message);
            }
        }

        #region Pool
        private static Stack<MessageObject> m_MessageObjectPool = new Stack<MessageObject>();
        private static int m_TotalCount;

        public static int TotalMessageObjectCount { get { return m_TotalCount; } }

        private static MessageObject GetCachedMessageObject()
		{
			MessageObject message = null;
			if(m_MessageObjectPool.Count == 0)
			{
				m_TotalCount++;
				message = new MessageObject(0, 0, null);
			}
			else
			{
				message = m_MessageObjectPool.Pop();
			}
			return message;
		}

		private static void RecycleMessageObject(MessageObject message)
		{
			if(message != null)
			{
				m_MessageObjectPool.Push(message);
			}
		}
		#endregion

	}
}
