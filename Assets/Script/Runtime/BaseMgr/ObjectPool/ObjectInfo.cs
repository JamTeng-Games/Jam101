//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Jam.Runtime.ObjectPool
{

    /// <summary>
    /// 对象信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public struct ObjectInfo
    {
        private readonly string _name;
        private readonly bool _locked;
        private readonly bool _customCanReleaseFlag;
        private readonly int _priority;
        private readonly DateTime _lastUseTime;
        private readonly int _spawnCount;

        public string Name => _name;
        public bool Locked => _locked;
        public bool CustomCanReleaseFlag => _customCanReleaseFlag;
        public int Priority => _priority;
        public DateTime LastUseTime => _lastUseTime;
        public bool IsInUse => _spawnCount > 0;

        public ObjectInfo(string name,
                          bool locked,
                          bool customCanReleaseFlag,
                          int priority,
                          DateTime lastUseTime,
                          int spawnCount)
        {
            _name = name;
            _locked = locked;
            _customCanReleaseFlag = customCanReleaseFlag;
            _priority = priority;
            _lastUseTime = lastUseTime;
            _spawnCount = spawnCount;
        }

        public int SpawnCount => _spawnCount;
    }

}