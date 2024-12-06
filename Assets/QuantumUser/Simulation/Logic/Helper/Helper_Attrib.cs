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
        }

        public static bool TryGetAttribValue(Frame f, EntityRef entity, AttributeType attribType, out int value)
        {
            value = 0;
            if (f.TryGet<AttribComp>(entity, out var attribComp))
            {
                value = GetAttribValue(f, attribComp, attribType);
                return true;
            }
            return false;
        }

        public static int GetAttribValue(Frame f, in AttribComp attribComp, AttributeType attribType)
        {
            var baseAttrib = f.ResolveDictionary(attribComp.BaseAttribs);
            var percentAttrib = f.ResolveDictionary(attribComp.PercentAttribs);
            return GetAttribValue(baseAttrib, percentAttrib, attribType);
        }

        public static int GetAttribValue(in QDictionary<int, int> baseAttrib,
                                         in QDictionary<int, FP> percentAttrib,
                                         AttributeType attribType)
        {
            var baseValue = baseAttrib[(int)attribType];
            var percentValue = percentAttrib[(int)attribType];
            int value = (baseValue * (1 + percentValue)).AsInt;
            return value;
        }
    }

}