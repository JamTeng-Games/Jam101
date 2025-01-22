namespace Jam.Runtime.Net_
{

    public enum NetCmd : ushort
    {
        // CS 客户端发到服务器
        CS_START = 1,
        CS_Login = 2,     // 登陆
        CS_Register = 3,  // 注册
        CS_Reconnect = 4, // 重连

        CS_ChangeName = 11,      // 修改名字
        CS_RefreshShop = 12,     // 刷新商店
        CS_BuyGoods = 13,        // 购买道具
        CS_ChooseHero = 14,      // 选择英雄
        CS_CreateRoom = 15,      // 创建房间
        CS_QueryRoomList = 16,   // 请求查询房间列表
        CS_JoinRoom = 17,        // 请求加入房间
        CS_RoomChat = 18,        // 房间聊天
        CS_RoomUserReady = 19,   // 房间准备
        CS_RoomStartBattle = 20, // 房间进入战斗
        CS_LeaveRoom = 21,       // 离开房间

        CS_END = 999,
        //
        // SC 服务器发到客户端
        SC_START = 1000,

        SC_AlreadyLogin = 1001,     // 已经登陆过
        SC_Kick = 1002,             // 顶替下线
        SC_LoginFail = 1003,        // 登陆失败
        SC_LoginSucc = 1004,        // 登陆成功
        SC_RegisterSucc = 1005,     // 注册成功
        SC_RegisterFail = 1006,     // 注册失败
        SC_NotifyRoleData = 1007,   // 下发用户角色数据
        SC_SyncTime = 1008,         // 同步时间
        SC_NotifyChooseHero = 1009, // 选择英雄

        //
        SC_NotifyMoneyData = 1010, // 下发用户钱包数据
        SC_NotifyItemData = 1011,  // 下发用户背包数据
        SC_NotifyRoundData = 1012, // 下发用户回合数据
        SC_MoneyAdd = 1020,        // 加钱
        SC_MoneyCost = 1021,       // 扣钱

        SC_ShopCreate = 1030,      // 商店创建
        SC_ShopDestroy = 1031,     // 商店销毁
        SC_ShopUpdateGoods = 1032, // 商店物品更新
        SC_ShopRefresh = 1033,     // 商店刷新
        SC_ShopRemoveGoods = 1034, // 商店移除道具

        SC_ItemAdd = 1040,    // 物品添加
        SC_ItemRemove = 1041, // 物品移除

        SC_CreateRole = 1050,        // 通知创角
        SC_CreateRoleFailed = 1051,  // 创角失败
        SC_CreateRoleSuccess = 1052, // 创角成功
        SC_EnterGame = 1053,         // 进入游戏

        SC_CreateRoomFail = 1060, // 创建房间失败
        SC_JoinRoomSucc = 1061,   // 加入房间成功
        SC_JoinRoomFail = 1062,   // 加入房间失败
        SC_LeaveRoomSucc = 1063,  // 离开房间成功
        SC_LeaveRoomFail = 1064,  // 离开房间失败
        SC_RoomUserEnter = 1065,  // 用户进入房间
        SC_RoomUserLeave = 1066,  // 用户离开房间
        SC_RoomUserUpdate = 1067, // 房间用户更新
        SC_RoomUserChat = 1068,   // 房间用户聊天
        SC_RoomUserInfos = 1069,  // 房间的全部用户信息
        SC_RoomDestroy = 1070,    // 房间销毁
        SC_RoomList = 1071,       // 房间列表
        SC_StartBattle = 1072,    // 战斗开始

        SC_END = 1999,
    }

/*



*/

}