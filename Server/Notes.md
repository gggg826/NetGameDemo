#Notes


---

###粘包
 1. TCP在传输数据时会把数据分包来传，所以就产生了长度粘包概念
 2. 所谓长度粘包就是数据+数据长度转成二进制
 3. 发送序列化好的消息前，要先粘包编码后再发送消息；同样，接受消息后，要粘包解码之后再解析数据（反序列化）

---

###接口
1. BitConverter接口可以转换基本类型和byte[]
2. MemoryStream.GetBuffer()返回从其创建此流的无符号字节数组
3. MemoryStream.ToArray()将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。常与2一起使用？