using System;
using System.Net.Sockets;

namespace SocketSystem
{
    public class UserToken
    {
        public Socket ConnectSocket;
        public SocketAsyncEventArgs ReceiveSAEA;
        public SocketAsyncEventArgs SendSAEA;

        public UserToken()
        {

        }

        public void ReceiveMsg(byte[] bytes)
        {

        }

        public void Close()
        {

        }

        public void Writed()
        {

        }
    }
}
