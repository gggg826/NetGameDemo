using System;
using Protocol;
using SocketSystem;

namespace Client
{
    public class HandlerManager : MessageEventHandler
    {
        public HandlerManager()
        {

        }

        public override void ReceiveMessage(object message)
        {
            MessageObject obj = message as MessageObject;
            Console.WriteLine("接收到服务器发来的消息");
        }
    }
}
