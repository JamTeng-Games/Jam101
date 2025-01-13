using System;
using System.Collections.Generic;
using Jam.Runtime.Net_;
using UnityEngine.Serialization;

namespace Jam.Runtime.Data_
{

    [Serializable]
    public class EmptyMsg
    {
        public byte data;

        public static EmptyMsg Data = new EmptyMsg();
    }

    // 请求注册
    [Serializable]
    public class RegisterReq
    {
        public string account; // 玩家名称
        public string pwd;     // 密码
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

    [Serializable]
    public class ItemBagData
    {
        public List<BagItem> item_bag;
    }

    [Serializable]
    public class MoneyBagData
    {
        public List<BagMoney> money_bag;
    }

    [Serializable]
    public class BagItem
    {
        public int id;
        public int amount;
    }

    [Serializable]
    public class BagMoney
    {
        public int id;
        public int amount;
    }

    [Serializable]
    public class RoleData
    {
        public string rid;
        public string name;
        public int hero;
    }

    [Serializable]
    public class ShopData
    {
        public int id;
        public List<ShopGoods> goods;
        public ShopRefreshMoney refresh_money;
    }

    [Serializable]
    public class ShopGoods
    {
        public int id;
        public int item_id;
        public int price;
        public int amount;
    }

    [Serializable]
    public class ShopRefreshMoney
    {
        public int money_id;
        public int price;
    }

    [Serializable]
    public class ShopGoodsUpdate
    {
        public int shop_id;  // 商店id
        public int goods_id; // 商品id
        public int amount;
    }

    [Serializable]
    public class SyncTimeData
    {
        public long time;
    }

    [Serializable]
    public class ChangeNameReq
    {
        public string name;
    }

    [Serializable]
    public class RoundData
    {
        public int round;
    }

    [Serializable]
    public class BuyGoodsReq
    {
        public int id;
        public int amount;
    }
    
    [Serializable]
    public class ChooseHeroReq
    {
        public int id;
    }

    [Serializable]
    public class ChooseHeroRes
    {
        public int hero;
    }

}