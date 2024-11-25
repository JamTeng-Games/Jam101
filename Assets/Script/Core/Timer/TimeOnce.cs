using System;
using UnityEngine;

namespace Jam.Core
{

    public class TimeOnce
    {
        private struct TimeOnceItem
        {
            public uint id;
            public float time;
            public Action callBack;
        }

        private const uint DefaultSize = 64;

        private TimeOnceItem[] _list;
        private uint _wPos;
        private uint _rPos;
        private uint _cap;
        private uint _id;

        public TimeOnce()
        {
            _list = new TimeOnceItem[DefaultSize];

            _cap = DefaultSize;
            _rPos = 0;
            _wPos = 0;
            _id = 0;
        }

        public uint Add(float time, Action callback)
        {
            Grow();
            _id++;

            var idx = _wPos % _list.Length;

            var id = _id;
            _list[idx].id = id;
            _list[idx].callBack = callback;
            _list[idx].time = Time.time + time;

            for (uint i = _wPos; i > _rPos; i--)
            {
                var idx1 = (i - 1) % _list.Length;
                var idx2 = i % _list.Length;
                if (_list[idx1].time <= _list[idx2].time)
                    break;

                var _item = _list[idx1];
                _list[idx1] = _list[idx2];
                _list[idx2] = _item;
            }

            _wPos++;
            return id;
        }

        public bool Remove(uint id)
        {
            for (uint i = _rPos; i < _wPos; i++)
            {
                if (_list[i % _cap].id == id)
                {
                    //Array.Copy(_list, i + 1, _list, i, _pos - i);
                    _list[i % _cap].callBack = null;
                    return true;
                }
            }
            return false;
        }
        
        public void Tick()
        {
            if (_wPos == 0)
                return;

            var now = Time.time;

            for (uint i = _rPos; i < _wPos; i++)
            {
                var idx = i % _list.Length;
                if (_list[idx].time > now)
                    return;
                if (_list[idx].callBack != null)
                {
                    try
                    {
                        _list[idx].callBack();
                    }
                    catch (Exception e)
                    {
                        JLog.Exception(e);
                    }
                }

                _rPos++;
            }
        }

        private void Grow()
        {
            if (_wPos - _rPos + 1 < _list.Length)
                return;

            uint newCap = _cap * 2;
            TimeOnceItem[] tmp = new TimeOnceItem[newCap];

            for (uint i = _rPos; i < _wPos; i++)
            {
                tmp[i % newCap] = _list[i % _cap];
            }
            _cap = newCap;
            _list = tmp;
        }

        public void Clean()
        {
            _rPos = 0;
            _wPos = 0;
        }
    }

}