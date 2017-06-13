/********************************************************************
*	created:	11/6/2017   2:46
*	filename: 	serializeutil
*	author:		Bing Lau
*	
*	purpose:	只序列化/反序列化消息的内容(.proto文件中的message类型)，不包含协议
*	
*********************************************************************/

using System;
using System.IO;

namespace Protocol
{
    public class SerializeUtil
    {
        private static PROTO m_Proto;

        private static PROTO GetPROTOObject()
        {
            if (m_Proto == null)
                m_Proto = new PROTO();
            return m_Proto;
        }

        /// <summary>
        /// 序列化message
        /// </summary>
        /// <typeparam name="T">message类型</typeparam>
        /// <param name="obj">message</param>
        /// <returns></returns>
        public static byte[] SerializeProto<T>(object obj)
        {
            if (obj == null)
                return null;

            byte[] buff = null;
			Type type = typeof(T);
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    PROTO proto = GetPROTOObject();
                    if (proto.IsDefined(typeof(T)))
                    {
                        proto.Serialize(ms, obj);
                        buff = ms.GetBuffer();
                        buff = ms.ToArray();
                        buff = buff.Length == 0 ? null : buff;
                    }
					else
					{
						Console.WriteLine(string.Format("序列化 {0} 失败! Protobuf中并未定义此类型", type));
					}
                }
            }
            catch (Exception e)
            {
				Console.WriteLine(string.Format("序列化 {0} 失败! {1}", type, e.Message));
			}
            return buff;
        }

        /// <summary>
        /// 反序列化message
        /// </summary>
        /// <typeparam name="T">message类型</typeparam>
        /// <param name="buff">message的byte[]</param>
        /// <returns></returns>
        public static object DeSerializeProto<T>(byte[] buff)
        {
            if (buff == null)
                return null;

            object obj = null;
			Type type = typeof(T);
			try
            {
                using (MemoryStream ms = new MemoryStream(buff))
                {
                    PROTO proto = GetPROTOObject();
                    if (proto.IsDefined(type))
                    {
                        obj = proto.Deserialize(ms, obj, type);
                    }
					else
					{
						Console.WriteLine(string.Format("序列化 {0} 失败! Protobuf中并未定义此类型", type));
					}
				}
            }
            catch (Exception e)
            {
				Console.WriteLine(string.Format("序列化 {0} 失败! {1}", type, e.Message));
			}
            return obj;
        }
    }
}
