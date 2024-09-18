using System;

namespace J.Core
{

    public class TimerFrameTicker
    {
        private struct FrameTickerItem
        {
            public uint id;
            public int curTimes;
            public int times;
            public Action<int> callBack;

            public bool Invoke()
            {
                callBack?.Invoke(curTimes);
                curTimes++;
                return curTimes >= times;
            }
        }

        private const int DefaultSize = 8;

        private FrameTickerItem[] _list;
        private FrameTickerItem[] _oldList;
        private int _cap;
        private int _pos;
        private int _oldPos;
        private uint _id;
        private bool _running;

        public TimerFrameTicker()
        {
            _list = new FrameTickerItem[DefaultSize];
            _oldList = new FrameTickerItem[DefaultSize];

            _pos = 0;
            _id = 0;
        }

        public uint Add(int times, Action<int> callback)
        {
            Grow();
            _id++;
            var id = _id;
            _list[_pos].id = id;
            _list[_pos].times = times;
            _list[_pos].callBack = callback;
            _pos++;
            return id;
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

            FrameTickerItem[] tmp = new FrameTickerItem[_list.Length * 2];
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