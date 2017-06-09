using System.Collections.Generic;
using System.Net.Sockets;

namespace SocketSystem
{
    public class UserToken
    {
        public Socket ConnectSocket;
        public SocketAsyncEventArgs ReceiveSAEA;
        public SocketAsyncEventArgs SendSAEA;
		public HandlerManagerBase HandlerManager;

		private List<byte> m_RecieveCache;
		private bool m_IsReading;

		private Queue<byte[]> m_SendQueue;
		private bool m_IsSending;

        public UserToken()
        {
			ReceiveSAEA = new SocketAsyncEventArgs();
			SendSAEA = new SocketAsyncEventArgs();

			m_IsReading = false;
			m_RecieveCache = new List<byte>();
			m_IsSending = false;
			m_SendQueue = new Queue<byte[]>();
        }

        public void ReceiveBytes(byte[] bytes)
        {
			m_RecieveCache.AddRange(bytes);
			if(!m_IsReading)
			{
				m_IsReading = true;
				ReadingMessage();
			}
		}

        public void Close()
        {

        }

        public void Writed()
        {

        }

		private void ReadingMessage()
		{
			byte[] buff = EncodUtil.Decode(ref m_RecieveCache);
			if(buff == null)
			{
				m_IsReading = false;
				return;
			}
			//反序列化buff
			object message = new object();//TODO
			HandlerManager.ReceiveMsg(this, message);
			ReadingMessage();
		}
    }

	public class UserTokenPool
	{
		private static Stack<UserToken> m_Pool;
		public static UserToken GetOne()
		{
			if (m_Pool == null)
				m_Pool = new Stack<UserToken>();
			if (m_Pool.Count == 0)
			{
				UserToken token = new UserToken();
				m_Pool.Push(token);
				return token;
			}
			else
				return m_Pool.Pop();

		}

		public static void ReturnOne(UserToken token)
		{
			if (m_Pool == null)
				m_Pool = new Stack<UserToken>();
			m_Pool.Push(token);
		}
	}
}
