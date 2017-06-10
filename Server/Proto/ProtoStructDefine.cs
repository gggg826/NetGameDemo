/********************************************************************
*	created:	11/6/2017   1:25
*	filename: 	ProtoStructDefine
*	author:		Bing Lau
*	
*	purpose:	定义一些网络数据传输中用到的Struct以及获取它们的长度
*	
*********************************************************************/

using System.Runtime.InteropServices;

namespace Proto
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Package_Head
    {
        public byte CmdType;
        public byte CmdID;

        public Package_Head(byte cmdType, byte cmdID)
        {
            CmdType = cmdType;
            CmdID = cmdID;
        }
    }

    public class ProtoStructDefine
    {
        public static readonly ushort Package_head_Length = (ushort)Marshal.SizeOf(typeof(Package_Head));
    }
}
