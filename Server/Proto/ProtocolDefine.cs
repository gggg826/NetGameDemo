/********************************************************************
*
*	file base:	ProtocolDefine
*	
*	purpose:	注册传输协议（传输协议要先注册再使用）
*	
*	created:	BingLau 
				13/6/2017   11:51
*********************************************************************/

namespace Protocol
{
    public class ProtocolDefine
    {
        public static void InitializeTankNetProtocolCM()
        {
            ProtocolManager.AddReceiveProtocol<PROTO_ROLE.TCM_CREATE_ROLE>((byte)PROTO_CMD_TYPE.CMD_TYPE.CMD_TYPE_ROLE, (byte)PROTO_ROLE.CLT_CMD.CM_CREATE_ROLE);

			ProtocolManager.AddSendProtocol<PROTO_ROLE.TCM_CREATE_ROLE>((byte)PROTO_CMD_TYPE.CMD_TYPE.CMD_TYPE_ROLE, (byte)PROTO_ROLE.CLT_CMD.CM_CREATE_ROLE);
		}
	}
}
