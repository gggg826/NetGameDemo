/********************************************************************
*
*	file base:	HandlerInterface
*	
*	purpose:	定义游戏各模块功能的接口
*	
*	created:	BingLau 
				13/6/2017   11:57
*********************************************************************/

using Protocol;
using SocketSystem;

namespace Server
{
    interface HandlerInterface
    {
        void ClientClose(UserToken token, string error);

        //   void ClientConnect(UserToken token);

        void MessageReceive(UserToken token, MessageObject message);
    }
}
