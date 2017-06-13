/********************************************************************
*
*	file base:	UserToken
*	
*	purpose:	接收、发送数据处理
*	
*	created:	BingLau 
				13/6/2017   12:03
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Protocol;

namespace SocketSystem
{
    public class UserToken
    {
        public delegate void ProcessSend(SocketAsyncEventArgs e);
        public ProcessSend OnProcessSend;
        public delegate void CloseProcess(UserToken token, string error);
        public CloseProcess OnCloseProcess;

        public Socket ConnectSocket;
        public SocketAsyncEventArgs ReceiveSAEA;
        public SocketAsyncEventArgs SendSAEA;
        public HandlerManagerBase HandlerManager;

        private List<byte> m_RecieveCache;
        private bool m_IsReceiving;

        private Queue<byte[]> m_SendQueue;
        private bool m_IsSending;

        public UserToken()
        {
            ReceiveSAEA    = new SocketAsyncEventArgs();
            ReceiveSAEA.UserToken = this;
            ReceiveSAEA.SetBuffer(new byte[1024], 0, 1024);
            SendSAEA       = new SocketAsyncEventArgs();
            SendSAEA.UserToken = this;

            m_IsReceiving    = false;
            m_RecieveCache = new List<byte>();
            m_IsSending    = false;
            m_SendQueue    = new Queue<byte[]>();
        }

        public void ReceiveBytes(byte[] bytes)
        {
            m_RecieveCache.AddRange(bytes);
            if (!m_IsReceiving)
            {
                m_IsReceiving = true;
                ReadMessage();
            }
        }

        public void SendBytes()
        {
            if (m_SendQueue.Count == 0)
            {
                m_IsSending = false;
                return;
            }

            byte[] buff = m_SendQueue.Dequeue();
            SendSAEA.SetBuffer(buff, 0, buff.Length);
            bool result = ConnectSocket.SendAsync(SendSAEA);
            if (!result)
            {
                if (OnProcessSend != null)
                    OnProcessSend(SendSAEA);
            }
        }

        public void Close()
        {
			try
			{
				m_SendQueue.Clear();
				m_RecieveCache.Clear();
				m_IsReceiving = false;
				m_IsSending = false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
        }

        public void OnSent()
        {
            SendBytes();
        }
        
        public void WriteMessage<T>(MessageObject obj)
        {
            if (ConnectSocket == null)
                return;

            byte[] buff = ProtocolManager.GetBuffFromMessageObject<T>(obj);
            byte[] sendBuff = EncodUtil.Encode(buff);
            m_SendQueue.Enqueue(sendBuff);
            if(!m_IsSending)
            {
                m_IsSending = true;
                SendBytes();
            }
        }

        private void ReadMessage()
        {
            byte[] buff = EncodUtil.Decode(ref m_RecieveCache);
            if (buff == null)
            {
                m_IsReceiving = false;
                return;
            }
            //反序列化buff
            object message = ProtocolManager.GetMessageObjectFromBuff(buff);
            HandlerManager.ReceiveMessage(this, message);
            ReadMessage();
        }


        public static UserToken GetUserTokenObject(/*Socket socket*/)
        {
            UserToken token = GetCachedUserTokenObject();
            //token.ConnectSocket = socket;
            return token;
        }

        public static void ReleaseUserTokenObject(UserToken token)
        {
            if (token != null)
            {
                //token.ConnectSocket = null;
                RecycleUserTokenObject(token);
            }

        }

        #region Pool
        private static Stack<UserToken> m_UserTokenPool = new Stack<UserToken>();
        private static int m_TotalUserTokenObjectCount;
        public static int ToTalUserTokenObjectCount { get { return m_TotalUserTokenObjectCount; } }

        private static UserToken GetCachedUserTokenObject()
        {
            UserToken token = null;
            if (m_UserTokenPool.Count == 0)
            {
                m_TotalUserTokenObjectCount++;
                token = new UserToken();
            }
            else
            {
                token = m_UserTokenPool.Pop();
            }
            return token;
        }

        private static void RecycleUserTokenObject(UserToken token)
        {
            if (token != null)
            {
                m_UserTokenPool.Push(token);
            }
        }
    }

    #endregion
}
