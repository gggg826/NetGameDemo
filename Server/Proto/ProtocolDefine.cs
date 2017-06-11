using ProtoBuf;
using ProtoBuf.Meta;

namespace Protocol
{
    public class ProtocolDefine
    {
        public static void InitializeTankNetProtocolCM()
        {
            ProtocolManager.AddReceiveProtocol<PROTO_ROLE.TCM_CREATE_ROLE>(PROTO_CMD_TYPE.CMD_TYPE.CMD_TYPE_ROLE, PROTO_ROLE.CLT_CMD.CM_CREATE_ROLE);

        }
    }
}
