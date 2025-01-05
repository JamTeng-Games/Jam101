namespace Jam.Runtime.Net_
{

    public enum NetErrorCode : ushort
    {
        None = 0,
        Unknow = 1,
        Timeout = 2, // 超时

        Login_WrongPwd = 21,            // 密码错误
        Login_NameExist = 22,           // 名称存在
        Login_NameTooLong = 23,         // 名称过长
        Login_PwdTooLong = 24,          // 名称过长
        Login_NotRegister = 25,         // 尚未注册
        Login_ReqError = 26,            //  请求失败
        Login_DisconnectWhenLogin = 27, // 未完成登陆即下线
        Login_ReplaceKick = 28          // 顶替下线
    }

}