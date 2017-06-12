using System;
using System.Collections.Generic;

namespace SocketSystem
{
    public class BuffEncodUtil
    {
        public static byte[] Encode(byte[] buff)
        {
			if (buff == null)
				return null;

			int int32Length = sizeof(int);
            byte[] lenghtBuff = BitConverter.GetBytes(buff.Length);

            byte[] result = new byte[buff.Length + int32Length];
            Array.Copy(lenghtBuff, 0, result, 0, int32Length);
            Array.Copy(buff, 0, result, int32Length, buff.Length);
            return result;
        }

        public static byte[] Decode(ref List<byte> cache)
        {
			if (cache.Count == 0)
				return null;

            byte[] cachedBuff = cache.ToArray();
            int int32length = sizeof(int);
            if (cachedBuff.Length < int32length)
                return null;
            byte[] messageLengthBuff = new byte[int32length];
            Buffer.BlockCopy(cachedBuff, 0, messageLengthBuff, 0, int32length);

            int messageLength = BitConverter.ToInt32(messageLengthBuff, 0);
            if (cachedBuff.Length - int32length < messageLength)
                return null;
            
            cache.Clear();
            int remainLength = cachedBuff.Length - messageLength - int32length;
            byte[] remainBuff = new byte[remainLength];
            Buffer.BlockCopy(cachedBuff, messageLength + int32length, remainBuff, 0, remainLength);
            cache.AddRange(remainBuff);

            byte[] resultBuff = new byte[messageLength];
            Buffer.BlockCopy(cachedBuff, int32length, resultBuff, 0, messageLength);
            return resultBuff;  
        }
    }
}
