//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using Jam.Core;

namespace Jam.Runtime.ObjectPool
{

    /// <summary>
    /// 对象基类。
    /// </summary>
    public abstract class ObjectBase : IReference
    {
        private string _name;
        private object _target;
        private bool _locked;
        private int _priority;
        private DateTime _lastUseTime;

        public string Name => _name;
        public object Target => _target;
        public bool Locked => _locked;
        public int Priority => _priority;
        public DateTime LastUseTime => _lastUseTime;
        public virtual bool CustomCanReleaseFlag => true;

        public ObjectBase()
        {
            _name = null;
            _target = null;
            _locked = false;
            _priority = 0;
            _lastUseTime = default(DateTime);
        }

        protected void Initialize(object target)
        {
            Initialize(null, target, false, 0);
        }

        protected void Initialize(string name, object target)
        {
            Initialize(name, target, false, 0);
        }

        protected void Initialize(string name, object target, bool locked)
        {
            Initialize(name, target, locked, 0);
        }

        protected void Initialize(string name, object target, int priority)
        {
            Initialize(name, target, false, priority);
        }

        protected void Initialize(string name, object target, bool locked, int priority)
        {
            if (target == null)
            {
                throw new Exception(Util.Text.Format("Target '{0}' is invalid.", name));
            }

            _name = name ?? string.Empty;
            _target = target;
            _locked = locked;
            _priority = priority;
            _lastUseTime = DateTime.UtcNow;
        }

        public void SetLocked(bool locked)
        {
            _locked = locked;
        }

        public void SetPriority(int priority)
        {
            _priority = priority;
        }

        public void SetLastUseTime(DateTime lastUseTime)
        {
            _lastUseTime = lastUseTime;
        }

        public virtual void Clean()
        {
            _name = null;
            _target = null;
            _locked = false;
            _priority = 0;
            _lastUseTime = default(DateTime);
        }

        public virtual void OnSpawn()
        {
        }

        public virtual void OnUnspawn()
        {
        }

        public abstract void Release(bool isShutdown);
    }

}