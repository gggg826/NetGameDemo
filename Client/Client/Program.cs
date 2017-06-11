using Protocol;
using ProtoBuf;
using SocketSystem;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerConnect.Instance.handler = new HandlerManager();
            ServerConnect.Instance.ServerStart();
            
            PROTO_ROLE.TCM_CREATE_ROLE role = new PROTO_ROLE.TCM_CREATE_ROLE();
            role.HeadIcon = 1;
            role.IsUnion = true;
            role.RoleName = "李雷";
            ServerConnect.Instance.WriteMessage<PROTO_ROLE.TCM_CREATE_ROLE>(new MessageObject((byte)PROTO_CMD_TYPE.CMD_TYPE.CMD_TYPE_ROLE, (byte)PROTO_ROLE.CLT_CMD.CM_CREATE_ROLE, role));
        }
    }
}
