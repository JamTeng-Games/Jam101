
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace Jam.Cfg
{
public sealed partial class Item : Luban.BeanBase
{
    public Item(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
        { if(!_buf["name"].IsString) { throw new SerializationException(); }  Name = _buf["name"]; }
        { if(!_buf["desc"].IsString) { throw new SerializationException(); }  Desc = _buf["desc"]; }
        { if(!_buf["quality"].IsNumber) { throw new SerializationException(); }  Quality = (ItemQuality)_buf["quality"].AsInt; }
        { if(!_buf["type"].IsNumber) { throw new SerializationException(); }  Type = (ItemType)_buf["type"].AsInt; }
        { if(!_buf["icon"].IsString) { throw new SerializationException(); }  Icon = _buf["icon"]; }
        { if(!_buf["money_id"].IsNumber) { throw new SerializationException(); }  MoneyId = _buf["money_id"]; }
        { if(!_buf["price"].IsNumber) { throw new SerializationException(); }  Price = _buf["price"]; }
        { var __json0 = _buf["add_skills"]; if(!__json0.IsArray) { throw new SerializationException(); } AddSkills = new System.Collections.Generic.List<int>(__json0.Count); foreach(JSONNode __e0 in __json0.Children) { int __v0;  { if(!__e0.IsNumber) { throw new SerializationException(); }  __v0 = __e0; }  AddSkills.Add(__v0); }   }
    }

    public static Item DeserializeItem(JSONNode _buf)
    {
        return new Item(_buf);
    }

    /// <summary>
    /// 这是id
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 名字
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 描述
    /// </summary>
    public readonly string Desc;
    /// <summary>
    /// 品质
    /// </summary>
    public readonly ItemQuality Quality;
    /// <summary>
    /// 类型
    /// </summary>
    public readonly ItemType Type;
    /// <summary>
    /// 图标
    /// </summary>
    public readonly string Icon;
    /// <summary>
    /// 货币类型
    /// </summary>
    public readonly int MoneyId;
    /// <summary>
    /// 单价
    /// </summary>
    public readonly int Price;
    /// <summary>
    /// 添加的技能 (可以添加多个)
    /// </summary>
    public readonly System.Collections.Generic.List<int> AddSkills;
   
    public const int __ID__ = 2289459;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "desc:" + Desc + ","
        + "quality:" + Quality + ","
        + "type:" + Type + ","
        + "icon:" + Icon + ","
        + "moneyId:" + MoneyId + ","
        + "price:" + Price + ","
        + "addSkills:" + Luban.StringUtil.CollectionToString(AddSkills) + ","
        + "}";
    }
}

}

