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
            ReceiveSAEA    = new SocketAsyncEventArgs();
            SendSAEA       = new SocketAsyncEventArgs();

            m_IsReading    = false;
            m_RecieveCache = new List<byte>();
            m_IsSending    = false;
            m_SendQueue    = new Queue<byte[]>();
        }

        public void ReceiveBytes(byte[] bytes)
        {
            m_RecieveCache.AddRange(bytes);
            if (!m_IsReading)
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
            if (buff == null)
            {
                m_IsReading = false;
                return;
            }
            //反序列化buff
            object message = new object();//TODO
            HandlerManager.ReceiveMsg(this, message);
            ReadingMessage();
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
