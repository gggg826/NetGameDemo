#Notes


---

###粘包
 1. TCP在传输数据时会把数据分包来传，所以就产生了长度粘包概念
 2. 所谓长度粘包就是数据+数据长度转成二进制
 3. 发送序列化好的消息前，要先粘包编码后再发送消息；同样，接受消息后，要粘包解码之后再解析数据（反序列化）
 4. Visio Packet  粘包
    Pack          组包
    Unpack        解包


---

###接口
1. BitConverter接口可以转换基本类型和byte[]
2. MemoryStream.GetBuffer()返回从其创建此流的无符号字节数组
3. MemoryStream.ToArray()将流内容写入字节数组，而与 System.IO.MemoryStream.Position 属性无关。常与2一起使用？
4. [StructLayout(LayoutKind.Sequential)] 内存中顺序布局

---

###设计技巧
1. 对象池可以与对象写在同一个类里，**对象池和对象池方法采用Static**, 取对象GetXXXObject, 回收对象ReleaseXXXObject，这两个方法对应写，Get时赋值的变量（通过传参）Release时全部置空或恢复默认值。对象池提供TotalXXXObjectCount属性、GetCachedXXXObject和RecycleXXXObject方法。
2. Socket发送数据时Push到Queue中，接收时每接收1byte放入List<byte>中

---

###SQLite
1. 连接路径：Data Source=|DataDirectory|\Data.db;Pooling=true;FailIfMissing=false
2. 右击项目-属性-目标平台 要选为X64，否则报错

