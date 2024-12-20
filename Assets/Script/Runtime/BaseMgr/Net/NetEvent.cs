namespace Jam.Runtime.Net_
{

    public enum NetEvent
    {
        ConnectSuccess, // 连接成功
        ConnectFailed,  // 连接失败
        Closed,         // 断开连接(上层主动断开)
        Disconnect,     // 断开连接(因为某些原因断开)
        BadPacket,      // 数据包破损
        BufferFull,     // 缓冲区满
        Unknown,        // 未知错误
    }

}