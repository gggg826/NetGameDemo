/********************************************************************
*
*	file base:	AccountHandler
*	
*	purpose:	帐户消息处理
*	
*	created:	BingLau 
				13/6/2017   12:04
*********************************************************************/

using Protocol;
using SocketSystem;

namespace Server.Handler
{
    public class AccountHandler : HandlerInterface
    {
        public void ClientClose(UserToken token, string error)
        {

        }

        public void MessageReceive(UserToken token, MessageObject message)
        {
			switch(message.CmdType)
			{
				case (byte)PROTO_ACCOUNT.CLT_CMD.CM_REGIST:
					break;
				case (byte)PROTO_ACCOUNT.CLT_CMD.CM_LOGIN:
					break;


				//这是服务端发往客户端的数据
				//case (byte)PROTO_ACCOUNT.SVR_CMD.SM_REGISTER_SUCCESSED:
				//	break;
				//case (byte)PROTO_ACCOUNT.SVR_CMD.SM_REGISTER_FAILED:
				//	break;
				//case (byte)PROTO_ACCOUNT.SVR_CMD.SM_LONGIN_SUCCESSED:
				//	break;
				//case (byte)PROTO_ACCOUNT.SVR_CMD.SM_LONGIN_FAILED:
				//	break;
			}
        }
    }
}
