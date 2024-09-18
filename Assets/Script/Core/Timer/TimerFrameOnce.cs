using System;

namespace J.Core
{

    public class TimerFrameOnce
    {
        private struct FrameOnceItem
        {
            public uint id;
            public Action callBack;
        }

        private const int DefaultSize = 8;

        private FrameOnceItem[] _list;
        private FrameOnceItem[] _oldList;
        private int _pos;
        private int _oldPos;
        private uint _id;

        public TimerFrameOnce()
        {
            _list = new FrameOnceItem[DefaultSize];
            _oldList = new FrameOnceItem[DefaultSize];

            _pos = 0;
            _id = 0;
        }

        public uint Add(Action callback)
        {
            Grow();
            _id++;
            var id = _id;
            _list[_pos].id = id;
            _list[_pos].callBack = callback;
            _pos++;
            return id;
        }

        public void Remove(uint id)
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
                    return;
                }
            }
        }

        public void Tick()
        {
            if (_pos == 0)
                return;

            _oldPos = _pos;
            _pos = 0;
            (_oldList, _list) = (_list, _oldList);

            for (int i = 0; i < _oldPos; i++)
            {
                try
                {
                    _oldList[i].callBack();
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

            FrameOnceItem[] tmp = new FrameOnceItem[_list.Length * 2];
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