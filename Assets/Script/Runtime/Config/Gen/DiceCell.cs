
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
/// <summary>
/// 掉落库
/// </summary>
public sealed partial class DiceCell : Luban.BeanBase
{
    public DiceCell(JSONNode _buf) 
    {
        { if(!_buf["item_id"].IsNumber) { throw new SerializationException(); }  ItemId = _buf["item_id"]; }
        { if(!_buf["weight"].IsNumber) { throw new SerializationException(); }  Weight = _buf["weight"]; }
    }

    public static DiceCell DeserializeDiceCell(JSONNode _buf)
    {
        return new DiceCell(_buf);
    }

    /// <summary>
    /// 道具id
    /// </summary>
    public readonly int ItemId;
    /// <summary>
    /// 权重
    /// </summary>
    public readonly int Weight;
   
    public const int __ID__ = -119717431;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "itemId:" + ItemId + ","
        + "weight:" + Weight + ","
        + "}";
    }
}

}

