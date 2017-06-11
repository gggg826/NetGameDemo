using SocketSystem;
using Protocol;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ProtocolDefine.InitializeTankNetProtocolCM();
            SocketSever server = new SocketSever(10);
            server.HandlerManager = new HandlerManager();
            server.ServerStart(6655);
            while (true)
            {
            }
        }
    }
}
