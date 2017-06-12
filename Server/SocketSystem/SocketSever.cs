using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//1.socket
//2.socketasynceventargs
//3.Semaphore 阻塞进程
//4.去借鉴抢滩的Protobuf管理

namespace SocketSystem
{
    public class SocketSever
    {
        public HandlerManagerBase HandlerManager;

        private Socket m_Socket;
        private Semaphore m_Semaphore;
        private int m_MaxClient;
        
        public SocketSever(int maxClient)
        {
            m_MaxClient = maxClient;
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_Semaphore = new Semaphore(m_MaxClient, m_MaxClient);
        }

        public void ServerStart(int port)
        {
			//初始化UserToken池不放在构造函数里是因为函数构造时外部还没有对Handler进行赋值
			for (int i = 0; i < m_MaxClient; i++)
			{
				UserToken token = new UserToken();
				token.HandlerManager = HandlerManager;
				token.OnProcessSend = ProcessSend;
				token.OnCloseProcess = ClientClose;
				token.ReceiveSAEA.Completed += OnIOCompleted;
				token.SendSAEA.Completed += OnIOCompleted;
				UserToken.ReleaseUserTokenObject(token);
			}

			try
            {
                m_Socket.Bind(new IPEndPoint(IPAddress.Any, port));
                m_Socket.Listen(m_MaxClient);
                StartAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ClientClose(UserToken token, string error)
        {
            if(token.ConnectSocket != null)
            {
                lock(token)
                {
                    HandlerManager.ClientClose(token, error);
                    token.Close();
                    UserToken.ReleaseUserTokenObject(token);
                    m_Semaphore.Release();
                }
            }
        }

        private void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
                e = null;

            m_Semaphore.WaitOne();
            bool result = m_Socket.AcceptAsync(e);
            if (!result)
                ProcessAccept(e);
        }


        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            UserToken token = UserToken.GetUserTokenObject();
            token.ConnectSocket = e.AcceptSocket;
            HandlerManager.ClientConnet(token);
            StartReceive(token);
            StartAccept(e);
        }

        private void StartReceive(UserToken token)
        {
            try
            {
                bool result = token.ConnectSocket.ReceiveAsync(token.ReceiveSAEA);
                if(!result)
                {
                    ProcessReceive(token.ReceiveSAEA);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if(token.ReceiveSAEA.BytesTransferred > 0 && token.ReceiveSAEA.SocketError == SocketError.Success)
            {
                byte[] bytes = new byte[token.ReceiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.ReceiveSAEA.Buffer, 0, bytes, 0, token.ReceiveSAEA.BytesTransferred);
                token.ReceiveBytes(bytes);
            }
            else
            {
                if (token.ReceiveSAEA.SocketError != SocketError.Success)
                    ClientClose(token, token.ReceiveSAEA.SocketError.ToString());
                else
                    ClientClose(token, "客户端断开链接...");
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (e.SocketError != SocketError.Success)
                ClientClose(token, e.SocketError.ToString());
            else
                token.OnSent();
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
                ProcessReceive(e);
            else
                ProcessSend(e);
        }
    }
}
