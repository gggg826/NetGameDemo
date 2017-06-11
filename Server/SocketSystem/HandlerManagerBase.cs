using Protocol;

namespace SocketSystem
{
    public abstract class HandlerManagerBase
    {
        public abstract void ClientConnet(UserToken token);

        public abstract void ReceiveMessage(UserToken token, object message);

        public abstract void ClientClose(UserToken token, string error);
    }
}
