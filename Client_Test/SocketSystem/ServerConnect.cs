using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Protocol;

namespace SocketSystem
{
    public class ServerConnect
    {
        private static ServerConnect m_Instance;
        public static ServerConnect Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new ServerConnect();
                return m_Instance;
            }
        }

        public MessageEventHandler handler;
        private readonly string IP = "127.0.0.1";
        private readonly int PORT  = 6655;

        private Socket m_socket;
        private byte[] m_Buff = new byte[1024];
        private int m_ConnectTryTime = 0;
        private List<byte> m_ReceiveCache;
        private Queue<byte[]> m_SendCache;
        private bool m_IsReading;
        private bool m_IsSending;

        public ServerConnect()
        {
            m_ReceiveCache = new List<byte>();
            m_SendCache    = new Queue<byte[]>();
            m_IsReading    = false;
            m_IsSending    = false;
            //ServerStart();
        }

        public void SendMessage()
        {
            if(m_SendCache.Count == 0)
            {
                m_IsSending = false;
                return;
            }

            byte[] buff = m_SendCache.Dequeue();
            try
            {
                m_socket.Send(buff);
            }
            catch (Exception e)
            {
                Console.WriteLine("没发送。。。");
            }
            SendMessage();
        }
        public void WriteMessage<T>(MessageObject obj)
        {
            if (m_socket == null)
            {
                Console.WriteLine("连接已断开");
                return;
            }

            byte[] buff = ProtocolManager.GetBuffFromMessageObject<T>(obj);
            byte[] sendBuff = BuffEncodUtil.Encode(buff);
            m_SendCache.Enqueue(sendBuff);
            if (!m_IsSending)
            {
                m_IsSending = true;
                SendMessage();
            }
        }

        public void ServerStart()
        {
            try
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_socket.Connect(IP, PORT);
                m_socket.BeginReceive(m_Buff, 0, 1024, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception)
            {
                if (++m_ConnectTryTime < 4)
                {
                    Console.WriteLine(string.Format("连接失败，尝试第{0}次重连...", m_ConnectTryTime));
                    ServerStart();
                }
                else
                {
                    Console.WriteLine("连接失败！！");
                    m_socket.Close();
                    m_socket = null;
                }
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            int length = m_socket.EndReceive(result);
            byte[] temp = new byte[length];
            Buffer.BlockCopy(m_Buff, 0, temp, 0, length);
            m_ReceiveCache.AddRange(temp);

            if (!m_IsReading)
            {
                m_IsReading = true;
                ReadMessage();
            }
            m_socket.BeginReceive(m_Buff, 0, 1024, SocketFlags.None, ReceiveCallback, null);
        }

        private void ReadMessage()
        {
            byte[] buff = BuffEncodUtil.Decode(ref m_ReceiveCache);
            if (buff == null)
            {
                m_IsReading = false;
                return;
            }
            //反序列化buff
            object message = ProtocolManager.GetMessageObjectFromBuff(buff);
            handler.ReceiveMessage(message);
            ReadMessage();
        }
        
    }
}
