/********************************************************************
*
*	file base:	HandlerManagerBase
*	
*	purpose:	提供外面游戏逻辑处理模块的接口类
*	
*	created:	BingLau 
				13/6/2017   12:01
*********************************************************************/

namespace SocketSystem
{
    public abstract class HandlerManagerBase
    {
        public abstract void ClientConnet(UserToken token);

        public abstract void ReceiveMessage(UserToken token, object message);

        public abstract void ClientClose(UserToken token, string error);
    }
}
