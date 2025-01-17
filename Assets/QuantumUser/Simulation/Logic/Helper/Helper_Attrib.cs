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
                        int valueWithStack = value * buff.stack;
                        Log.Debug($"Attrib {(AttributeType)attrType} {value} stack {buff.stack}");
                        if (valueAttrib.TryGetValue(attrType, out var v))
                        {
                            valueAttrib[attrType] = v + valueWithStack;
                        }
                        else
                        {
                            valueAttrib[attrType] = valueWithStack;
                        }
                    }

                    // Percent
                    foreach (var (attrType, perValue) in buffPercentAttrib)
                    {
                        FP valueWithStack = perValue * buff.stack;
                        if (percentAttrib.TryGetValue(attrType, out var v))
                        {
                            percentAttrib[attrType] = v + valueWithStack;
                        }
                        else
                        {
                            percentAttrib[attrType] = valueWithStack;
                        }
                    }
                }
            }

            if (f.Unsafe.TryGetPointer<AttribComp>(entity, out var attribComp2))
            {
                var valueAttrib = f.ResolveDictionary(attribComp2->ValueAttribs);
                var percentAttrib = f.ResolveDictionary(attribComp2->ValueAttribs);
                foreach (var (attrType, value) in valueAttrib)
                {
                    Log.Debug($"Attrib2 value {(AttributeType)attrType} {value}");
                }
                // Percent
                foreach (var (attrType, perValue) in percentAttrib)
                {
                    Log.Debug($"Attrib2 percent {(AttributeType)attrType} {perValue}"); 
                }
            }

            if (TryGetAttribValue(f, entity, AttributeType.MaxHp, out var speed))
            {
                Log.Debug($"Recalculate MaxHp {speed}");
            }
        }

        public static bool TryGetAttribValue(Frame f, EntityRef entity, AttributeType attribType, out int value)
        {
            value = Int32.MaxValue;
            if (f.TryGet<AttribComp>(entity, out var attribComp))
            {
                var valueAttrib = f.ResolveDictionary(attribComp.ValueAttribs);
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