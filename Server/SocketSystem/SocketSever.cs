using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            for (int i = 0; i < m_MaxClient; i++)
            {
                UserToken token = new UserToken();

            }
        }

        public void ClientStart(int port)
        {
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
                    UserTokenPool.ReturnOne(token);
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
            UserToken token = UserTokenPool.GetOne();
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
                token.ReceiveMsg(bytes);
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
                token.Writed();
        }

        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void OnIOCompleted(SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
                ProcessReceive(e);
            else
                ProcessSend(e);
        }
    }
}
