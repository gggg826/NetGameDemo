﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketSystem
{
    public abstract class MessageEventHandler
    {
        public abstract void ReceiveMessage(object message);
    }
}
