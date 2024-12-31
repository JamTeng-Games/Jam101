namespace Jam.Runtime.Net_
{

    public enum NetCmd : ushort
    {
        // CS 客户端发到服务器
        CS_START = 1,
        CS_Login = 2,
        CS_Reconnect = 3,

        CS_END = 999,

        // SC 服务器发到客户端
        SC_START = 1000,

        SC_AlreadyLogin = 1001,
        SC_Kick = 1002, // 顶替下线

        SC_END = 1999,
    }

/*



*/

}