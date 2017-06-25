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
            PROTO_ROLE.TCM_CREATE_ROLE role = obj.Message as PROTO_ROLE.TCM_CREATE_ROLE;
            Console.WriteLine(string.Format("接收到服务器发来的消息:\n角色名：{0}\n头像:{1}", role.RoleName, role.HeadIcon));
        }
    }
}
