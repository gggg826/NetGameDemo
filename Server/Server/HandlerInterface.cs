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
