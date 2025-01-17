using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jam.Cfg
{

    public enum BuffArgType
    {
        None,
        Bool,
        Int8,
        Int16,
        Int32,
        Int64,
        UInt8,
        UInt16,
        UInt32,
        UInt64,
        Fp,
        Vec2,
        Vec3,
        Str,
    }

    [Serializable]
    public struct BuffArg
    {
        public BuffArgType type;
        public string argName;
#if UNITY_EDITOR
        public string desc;
#endif
    }

    [CreateAssetMenu(fileName = "BuffTemplate", menuName = "Jam/BuffTemplate")]
    public class BuffTemplateSO : ScriptableObject
    {
#if UNITY_EDITOR
        [Title("中文名")] public string descName;
#endif

        [Title("英文名 (建议找程序商议)")]
        public new string name;

        [Title("参数类型以及参数名 (建议找程序商议)")]
        public List<BuffArg> args;

#if UNITY_EDITOR
        [Title("描述")]
        [HideLabel]
        [MultiLineProperty(10)]
        public string desc;
#endif
    }

}