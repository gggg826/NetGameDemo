using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketSystem
{
    public abstract class HandlerManagerBase
    {
        public abstract void ClientConnet(UserToken token);

        public abstract void ReceiveMsg(UserToken token, object message);

        public abstract void ClientClose(UserToken token, string error);
    }
}
