using System;
using System.Net.Sockets;
using System.Threading;

//1.socket
//2.socketasynceventargs
//3.Semaphore 阻塞进程
//4.去借鉴抢滩的Protobuf管理

namespace Server
{
    public class SocketSever
    {
        private Semaphore m_Semaphore;

        public SocketSever()
        {

        }

        public void ClientStart()
        {

        }

        public void ClientClose()
        {

        }

        private void StartAccept(SocketAsyncEventArgs e)
        {

        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {

        }

        private void StartReceive(SocketAsyncEventArgs e)
        {

        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {

        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {

        }

        private void OnAcceptCompleted(SocketAsyncEventArgs e)
        {

        }

        private void OnIOCompleted(SocketAsyncEventArgs e)
        {

        }
    }
}
