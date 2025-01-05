using System;
using System.Collections.Generic;

namespace Jam.Runtime.Net_
{

    // 请求注册
    [Serializable]
    public class RegisterReq
    {
        public string name; // 玩家名称
        public string pwd;  // 密码
    }

    // 注册成功
    [Serializable]
    public class RegisterSuccess
    {
        public string account; // 账号id
    }

    // 注册成功
    [Serializable]
    public class RegisterFail
    {
        public NetErrorCode code; // 错误原因
    }

    // 请求登录
    [Serializable]
    public class LoginReq
    {
        public string account; // 账号id
        public string pwd;     // 密码
    }

    // 登录成功
    [Serializable]
    public class LoginSuccess
    {
        public string account; // 账号id 
        public int token;
    }

    // 登录失败
    [Serializable]
    public class LoginFail
    {
        public NetErrorCode code; // 失败原因
    }

    // 已经登录
    [Serializable]
    public class AlreadyLogin
    {
        public string account; // 账号id 
        public int token;
    }

    // 角色数据
    [Serializable]
    public class UserData
    {
        public ulong rid;        // record id, 数据库中的记录id
        public string account;   // 账号id
        public string name;      // 玩家名称
        public ulong last_login; // 上次登录时间
        public ulong play_time;  // 累计游戏时间
        public int level;        // 玩家等级
        public int exp;          // 玩家经验

        public int round;       // 处于第几回合
        public int hero_id;     // 英雄配置id
        public int gold;        // 金币
        public List<int> items; // 背包物品
    }

}