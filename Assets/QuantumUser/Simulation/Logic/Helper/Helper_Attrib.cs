using System;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    /// <summary>
    /// 角色属性的帮助类
    /// </summary>
    public unsafe class Helper_Attrib
    {
        public static void Recalculate(Frame f, EntityRef entity)
        {
            Log.Debug($"Recalculate");
            if (f.Unsafe.TryGetPointer<AttribComp>(entity, out var attribComp) &&
                f.TryGet<BuffComp>(entity, out var buffComp))
            {
                var valueAttrib = f.ResolveDictionary(attribComp->ValueAttribs);
                var percentAttrib = f.ResolveDictionary(attribComp->PercentAttribs);
                var buffs = f.ResolveList(buffComp.Buffs);

                valueAttrib.Clear();
                percentAttrib.Clear();
                for (int i = 0; i < buffs.Count; i++)
                {
                    var buff = buffs[i];
                    var buffValueAttrib = f.ResolveDictionary(buff.model.valueAttribs);
                    var buffPercentAttrib = f.ResolveDictionary(buff.model.percentAttribs);
                    // Value
                    foreach (var (attrType, value) in buffValueAttrib)
                    {
                        Log.Debug($"Attrib {(AttributeType)attrType} {value}");
                        if (valueAttrib.TryGetValue(attrType, out var v))
                        {
                            valueAttrib[attrType] = v + value;
                        }
                        else
                        {
                            valueAttrib[attrType] = value;
                        }
                    }

                    // Percent
                    foreach (var (attrType, perValue) in buffPercentAttrib)
                    {
                        if (percentAttrib.TryGetValue(attrType, out var v))
                        {
                            percentAttrib[attrType] = v + perValue;
                        }
                        else
                        {
                            percentAttrib[attrType] = perValue;
                        }
                    }
                }
            }

            if (f.Unsafe.TryGetPointer<AttribComp>(entity, out var attribComp2))
            {
                var valueAttrib = f.ResolveDictionary(attribComp2->ValueAttribs);
                foreach (var (attrType, value) in valueAttrib)
                {
                    Log.Debug($"Attrib2 {(AttributeType)attrType} {value}");
                }
            }
        }

        public static bool TryGetAttribValue(Frame f, EntityRef entity, AttributeType attribType, out int value)
        {
            value = Int32.MaxValue;
            if (f.TryGet<AttribComp>(entity, out var attribComp))
            {
                var valueAttrib = f.ResolveDictionary(attribComp.ValueAttribs);
                foreach (var (k, v) in valueAttrib)
                {
                    Log.Debug($"TryGetAttribValue {(AttributeType)k}, {v}");
                }
                var percentAttrib = f.ResolveDictionary(attribComp.PercentAttribs);
                if (valueAttrib.TryGetValue((int)attribType, out var baseValue))
                {
                    if (percentAttrib.TryGetValue((int)attribType, out var percentValue))
                    {
                        value = (baseValue * (1 + percentValue)).AsInt;
                        return true;
                    }
                    value = baseValue;
                    return true;
                }
            }
            return false;
        }
    }

}