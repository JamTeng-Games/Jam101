using System;
using UnityEngine;

namespace Jam.Core
{

    public class TimeTicker
    {
        private struct TimeTickerItem
        {
            public uint id;
            public float gap;
            public float nextTime;
            public int curtime;
            public int times;
            public Action<int> callBack;

            public bool Invoke()
            {
                callBack?.Invoke(curtime);
                curtime++;
                return curtime >= times;
            }
        }

        private const int DefaultSize = 8;

        private TimeTickerItem[] _list;
        private TimeTickerItem[] _oldList;
        private TimeTickerItem _item;
        private int _cap;
        private int _pos;
        private int _oldPos;
        private uint _id;
        private bool _running;

        public TimeTicker()
        {
            _list = new TimeTickerItem[DefaultSize];
            _oldList = new TimeTickerItem[DefaultSize];

            _pos = 0;
            _id = 0;
        }

        public uint Add(float gap, int times, Action<int> callback)
        {
            if (times <= 0)
                return 0;

            Grow();
            _id++;
            _item.id = _id;
            _item.gap = gap;
            _item.times = times;
            _item.callBack = callback;
            _item.nextTime = Time.time + gap;
            _list[_pos] = _item;
            _pos++;
            return _item.id;
        }

        public bool Remove(uint id)
        {
            for (int i = 0; i < _pos; i++)
            {
                if (_list[i].id == id)
                {
                    for (int j = i + 1; j < _pos; j++)
                    {
                        _list[j - 1] = _list[j];
                    }
                    //Array.Copy(_list, i + 1, _list, i, _pos - i);
                    _pos--;
                    return true;
                }
            }
            return false;
        }

        public void Tick()
        {
            if (_pos <= 0)
                return;

            (_oldList, _list) = (_list, _oldList);
            _oldPos = _pos;
            _pos = 0;

            for (int i = 0; i < _oldPos; i++)
            {
                try
                {
                    if (_oldList[i].Invoke())
                    {
                        Remove(_oldList[i].id);
                        Grow();
                        _list[_pos] = _oldList[i];
                        _pos++;
                    }
                }
                catch (Exception e)
                {
                    JLog.Exception(e);
                }
            }
        }

        private void Grow()
        {
            if (_pos + 1 < _list.Length)
                return;

            TimeTickerItem[] tmp = new TimeTickerItem[_list.Length * 2];
            Array.Copy(_list, tmp, _pos);
            _list = tmp;
        }

        public void Clean()
        {
            _pos = 0;
            _oldPos = 0;
        }
    }

}