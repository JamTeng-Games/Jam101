
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using SimpleJSON;


namespace Jam.Cfg.test
{
/// <summary>
/// 这是个测试excel结构
/// </summary>
public sealed partial class TestExcelBean2 : Luban.BeanBase
{
    public TestExcelBean2(JSONNode _buf) 
    {
        { if(!_buf["y1"].IsNumber) { throw new SerializationException(); }  Y1 = _buf["y1"]; }
        { if(!_buf["y2"].IsString) { throw new SerializationException(); }  Y2 = _buf["y2"]; }
        { if(!_buf["y3"].IsNumber) { throw new SerializationException(); }  Y3 = _buf["y3"]; }
    }

    public static TestExcelBean2 DeserializeTestExcelBean2(JSONNode _buf)
    {
        return new test.TestExcelBean2(_buf);
    }

    /// <summary>
    /// 最高品质
    /// </summary>
    public readonly int Y1;
    /// <summary>
    /// 黑色的
    /// </summary>
    public readonly string Y2;
    /// <summary>
    /// 蓝色的
    /// </summary>
    public readonly float Y3;
   
    public const int __ID__ = -1738345159;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "y1:" + Y1 + ","
        + "y2:" + Y2 + ","
        + "y3:" + Y3 + ","
        + "}";
    }
}

}

