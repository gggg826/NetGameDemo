/********************************************************************
*
*	file base:	EncodUtil
*	
*	purpose:	粘包编码、解码
*	
*	created:	BingLau 
				13/6/2017   12:00
*********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

//Visio Packet  粘包
//Pack          组包
//Unpack        解包
	
namespace SocketSystem
{
	public class EncodUtil
	{
		/// <summary>
		/// 粘包长度编码
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static byte[] Encode(byte[] bytes)
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter sw = new BinaryWriter(ms);
			sw.Write(bytes.Length);
			sw.Write(bytes);
			byte[] result = new byte[ms.Length];
			Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int)ms.Length);
			sw.Close();
			ms.Close();
			return result;
		}

		/// <summary>
		/// 粘包长度解码
		/// 1.根据长度解码
		/// 2.cache中大于解码长度的部分放入cache供下个数据包使用，这也就是为什么参数要用ref
		/// </summary>
		/// <param name="cache"></param>
		/// <returns></returns>
		public static byte[] Decode(ref List<byte> cache)
		{
			if (cache.Count < 4)
				return null;
			MemoryStream ms = new MemoryStream(cache.ToArray());
			BinaryReader br = new BinaryReader(ms);
			int length = br.ReadInt32();
			if (ms.Length - ms.Position < length)
				return null;
			byte[] result = br.ReadBytes(length);
			cache.Clear();
			cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
			br.Close();
			ms.Close();
			return result;
		}
	}
}
