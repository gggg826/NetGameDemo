using SocketSystem;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketSever server = new SocketSever(10);
            server.HandlerManager = new HandlerManager();
            server.ServerStart(6655);
            while (true)
            {
            }
        }
    }
}
