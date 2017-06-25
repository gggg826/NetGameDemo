/********************************************************************
*	created:	11/6/2017   1:54
*	filename: 	byteutil
*	author:		Bing Lau
*	
*	purpose:	从其他地方搞过来的byte与各种类型数据的转换，System.BitConverter类的扩展
*	            其中参数offset为数据流中的position
*	            
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Protocol
{
    public class ByteUtil
    {
        public static bool IS_UNICODE_ENCODING = false;
        public static bool IS_UTF8_ENCODING = false;

        private static byte[] IntBuffer = new byte[sizeof(int)];
        private static byte[] UlongBuffer = new byte[sizeof(ulong)];
        private static byte[] ShortBuffer = new byte[sizeof(short)];
        private static byte[] FloatBuffer = new byte[sizeof(float)];

        public static object BytesToStuct(byte[] bytes, Type type, int size)
        {
            if (size > bytes.Length)
            {
                return null;
            }
            IntPtr destination = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, destination, size);
            object obj2 = Marshal.PtrToStructure(destination, type);
            Marshal.FreeHGlobal(destination);
            return obj2;
        }

        public static int BytesToInt(byte[] bytes, ref int offset)
        {
            int size = sizeof(int);
            Int32 i = 0;
            Array.Copy(bytes, offset, IntBuffer, 0, size);
            //i = (Int32)BytesToStuct(IntBuffer, typeof(Int32), size);
            i = System.BitConverter.ToInt32(IntBuffer, 0);
            offset += size;
            return i;
        }

        public static ulong BytesToUlong(byte[] bytes, ref int offset)
        {
            int size = sizeof(ulong);
            ulong ul = 0;
            Array.Copy(bytes, offset, UlongBuffer, 0, size);
            ul = System.BitConverter.ToUInt64(UlongBuffer, 0);
            offset += size;
            return ul;
        }

        public static short BytesToShort(byte[] bytes, ref int offset)
        {
            int size = sizeof(short);
            Int16 i = 0;
            Array.Copy(bytes, offset, ShortBuffer, 0, size);
            i = System.BitConverter.ToInt16(ShortBuffer, 0);
            offset += size;
            return i;
        }

        public static short BytesToUshort(byte[] bytes, ref int offset)
        {
            int size = sizeof(ushort);
            Int16 ui = 0;
            Array.Copy(bytes, offset, ShortBuffer, 0, size);
            ui = System.BitConverter.ToInt16(ShortBuffer, 0);
            offset += size;
            return ui;
        }

        public static float BytesToFloat(byte[] bytes, ref int offset)
        {
            int size = sizeof(float);
            Single f = 0;
            Array.Copy(bytes, offset, FloatBuffer, 0, size);
            //f = (Single)BytesToStuct(FloatBuffer, typeof(Single), size);
            f = System.BitConverter.ToSingle(FloatBuffer, 0);
            offset += size;
            return f;
        }

        public static string BytesToUTF8String(byte[] bytes)
        {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static bool Memcpy(ref List<char> buff, int buffIndex, char[] s, int charCount)
        {
            byte[] buffer = CharsToBytes(buff.ToArray());
            bool flag = Memcpy(ref buffer, buffIndex, s, charCount);
            if (flag)
            {
                buff = new List<char>((IEnumerable<char>)BytesToChars(buffer));
            }
            return flag;
        }

        public static bool Memcpy(ref byte[] buff, int buffIndex, char[] s, int charCount)
        {
            try
            {
                byte[] buffer = CharsToBytes(s);
                int index = buffIndex;
                for (int i = 0; i < charCount; i++)
                {
                    if ((index < buff.Length) && (i < buffer.Length))
                    {
                        buff[index] = buffer[i];
                    }
                    index++;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool Memmove(List<char> src, ulong srcIndex, ref List<char> dst, ulong dstIndex, ulong charCount)
        {
            char[] sourceArray = src.ToArray();
            char[] destinationArray = dst.ToArray();
            try
            {
                Array.Copy(sourceArray, (int)srcIndex, destinationArray, (int)dstIndex, (int)charCount);
            }
            catch
            {
                return false;
            }
            dst = new List<char>(destinationArray);
            return true;
        }

        public static int SafeSizeof(object Obj)
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(Obj);
        }

        public static byte[] StructToBytes(object structObj, int size)
        {
            byte[] destination = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, ptr, false);
            Marshal.Copy(ptr, destination, 0, size);
            Marshal.FreeHGlobal(ptr);
            return destination;
        }

        public static byte[] StringToBytes(string sv)
        {
            if (sv != null)
            {
                char[] cBuffer = sv.ToCharArray();
                byte[] bBuffer = Encoding.UTF8.GetBytes(cBuffer);
                return bBuffer;

            }
            return null;
        }

        public static byte[] ShortToBytes(short s)
        {
            Int16 i16 = new Int16();
            i16 = s;
            return StructToBytes(i16, sizeof(short));
        }

        public static byte[] UshortToBytes(ushort us)
        {
            UInt16 ui16 = new UInt16();
            ui16 = us;
            return StructToBytes(ui16, sizeof(ushort));
        }

        public static byte[] IntToBytes(int i)
        {
            Int32 i32 = new Int32();
            i32 = i;
            return StructToBytes(i32, sizeof(int));
        }

        public static byte[] UlongToBytes(ulong ul)
        {
            ulong ul64 = new ulong();
            ul64 = ul;
            return StructToBytes(ul64, sizeof(ulong));
        }

        public static byte[] FloatToBytes(float f)
        {
            Single s = new Single();
            s = f;
            return StructToBytes(s, sizeof(float));
        }

        public static byte[] CharsToBytes(char[] chr)
        {
            List<byte> list = new List<byte>();
            byte[] bytes = null;
            byte[] buffer2 = null;
            string str = "log = null ";
            if (null == chr)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            StringBuilder builder2 = new StringBuilder();
            if (IS_UTF8_ENCODING)
            {
                buffer2 = Encoding.UTF8.GetBytes(chr);
            }
            else if (IS_UNICODE_ENCODING)
            {
                bytes = Encoding.Unicode.GetBytes(chr);
            }
            else
            {
                int index = 0;
                index = 0;
                while (index < chr.Length)
                {
                    char ch = chr[index];
                    byte item = Convert.ToByte(ch);
                    list.Add(item);
                    index++;
                }
            }
           return list.ToArray();
        }

        public static char[] BytesToChars(byte[] bpara)
        {
            List<char> list = new List<char>();
            for (int i = 0; i < bpara.Length; i++)
            {
                byte num2 = bpara[i];
                list.Add(Convert.ToChar(num2));
            }
            return list.ToArray();
        }

        public static string BytesToString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                byte num2 = bytes[i];
                builder.Append("-");
                builder.Append(num2);
                i++;
            }
            return builder.ToString();
        }

        public static string CharsToString(char[] chr)
        {
            string str;
            List<byte> list = new List<byte>();
            if (null == chr)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            if (IS_UTF8_ENCODING)
            {
                str = BytesToString(Encoding.UTF8.GetBytes(chr));
            }
            else if (IS_UNICODE_ENCODING)
            {
                str = BytesToString(Encoding.Unicode.GetBytes(chr));
            }
            else
            {
                for (int i = 0; i < chr.Length; i++)
                {
                    char ch = chr[i];
                    byte item = Convert.ToByte(ch);
                    list.Add(item);
                    builder.Append("-");
                    builder.Append(item);
                    i++;
                }
                str = builder.ToString();
            }
            return str;
        }
    }
}
