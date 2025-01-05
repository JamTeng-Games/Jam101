namespace Jam.Runtime.Net_
{

    public enum NetCmd : ushort
    {
        // CS 客户端发到服务器
        CS_START = 1,
        CS_Login = 2,       // 登陆
        CS_Register = 3,    // 注册
        CS_Reconnect = 4,   // 重连

        CS_END = 999,
        //
        // SC 服务器发到客户端
        SC_START = 1000,

        SC_AlreadyLogin = 1001, // 已经登陆过
        SC_Kick = 1002,         // 顶替下线
        SC_LoginFail = 1003,    // 登陆失败
        SC_LoginSucc = 1004,    // 登陆成功
        SC_RegisterSucc = 1005, // 注册成功
        SC_RegisterFail = 1006, // 注册失败

        SC_END = 1999,
    }

/*



*/

}