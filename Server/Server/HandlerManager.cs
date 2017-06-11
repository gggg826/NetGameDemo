using System;
using Protocol;
using SocketSystem;
using Server.Handler;

namespace Server
{
    public class HandlerManager : HandlerManagerBase
    {
        private int m_ConnetCount;
        private HandlerInterface m_AccountHandler;
        private HandlerInterface m_RoleHandler;

        public HandlerManager()
        {
            m_AccountHandler = new AccountHandler();
            m_RoleHandler = new RoleHandler();
        }

        public override void ClientClose(UserToken token, string error)
        {
            m_ConnetCount--;
            Console.WriteLine("客户端断开连接");
            Console.WriteLine("当前在线人数： " + m_ConnetCount);
        }

        public override void ClientConnet(UserToken token)
        {
            m_ConnetCount++;
            Console.WriteLine("有客户端连接了");
            Console.WriteLine("当前在线人数： " + m_ConnetCount);
        }
        
        public override void ReceiveMessage(UserToken token, object message)
        {
            MessageObject obj = message as MessageObject;
            switch(obj.CmdType)
            {
                case (byte)PROTO_CMD_TYPE.CMD_TYPE.CMD_TYPE_ROLE:
                    m_RoleHandler.MessageReceive(token, obj);
                    break;
            }
        }
    }
}
