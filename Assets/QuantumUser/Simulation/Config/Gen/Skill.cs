
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace Quantum.Cfg_
{
public sealed partial class Skill : Luban.BeanBase
{
    public Skill(JSONNode _buf) 
    {
        { if(!_buf["id"].IsNumber) { throw new SerializationException(); }  Id = _buf["id"]; }
    }

    public static Skill DeserializeSkill(JSONNode _buf)
    {
        return new Skill(_buf);
    }

    /// <summary>
    /// 技能id
    /// </summary>
    public readonly int Id;
   
    public const int __ID__ = 79944241;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "}";
    }
}

}

