/********************************************************************
*
*	file base:	RoleHandler
*	
*	purpose:	角色消息处理
*	
*	created:	BingLau 
				13/6/2017   12:05
*********************************************************************/

using System;
using Protocol;
using SocketSystem;

namespace Server.Handler
{
    public class RoleHandler:HandlerInterface
    {
        public RoleHandler()
        {

        }

        public void ClientClose(UserToken token, string error)
        {

        }

        public void MessageReceive(UserToken token, MessageObject message)
        {
            Console.WriteLine("获取角色");
            PROTO_ROLE.TCM_CREATE_ROLE role = new PROTO_ROLE.TCM_CREATE_ROLE();
            role.HeadIcon = 1;
            role.IsUnion = true;
            role.RoleName = "小强";
            MessageObject obj = new MessageObject((byte)PROTO_CMD_TYPE.CMD_TYPE.CMD_TYPE_ROLE, (byte)PROTO_ROLE.CLT_CMD.CM_CREATE_ROLE, (object)role);
            token.WriteMessage<PROTO_ROLE.TCM_CREATE_ROLE>(obj);
        }
    }
}
