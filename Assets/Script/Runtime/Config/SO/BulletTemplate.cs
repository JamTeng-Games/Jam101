using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Jam.Cfg
{

    [CreateAssetMenu(fileName = "BulletTemplate", menuName = "Jam/BulletTemplate")]
    public class BulletTemplateSO : ScriptableObject
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