using System;

namespace J.Core
{

    public class TimerFrameLoop
    {
        private const int DefaultSize = 64;

        private struct FrameLoopItem
        {
            public uint id;
            public Action callBack;
        }

        private FrameLoopItem[] _list;
        private FrameLoopItem[] _oldList;
        private int _pos;
        private int _oldPos;
        private int _oldLen;
        private uint _id;

        public TimerFrameLoop()
        {
            _list = new FrameLoopItem[DefaultSize];
            _oldList = new FrameLoopItem[DefaultSize];

            _pos = 0;
            _oldPos = 0;
            _oldLen = 0;
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
            for (int i = 0; i < _oldLen; i++)
            {
                if (_oldList[i].id == id)
                {
                    for (int j = i + 1; j < _oldLen; j++)
                    {
                        _oldList[j - 1] = _oldList[j];
                    }
                    if (i <= _oldPos)
                    {
                        _oldPos--;
                    }
                    _oldLen--;
                    return;
                }
            }

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
            var tmp = _oldList;
            _oldList = _list;
            _list = tmp;
            _oldLen = _pos;
            _pos = 0;

            for (_oldPos = 0; _oldPos < _oldLen; _oldPos++)
            {
                try
                {
                    _oldList[_oldPos].callBack();
                }
                catch (Exception e)
                {
                    JLog.Exception(e);
                }
            }

            // 扩容oldList
            if (_pos > 0)
            {
                if (_oldList.Length <= _oldPos + _pos)
                {
                    int cap = _oldList.Length * 2;
                    while (cap < _oldPos + _pos)
                    {
                        cap *= 2;
                    }
                    tmp = new FrameLoopItem[cap];
                    Array.Copy(_oldList, tmp, _oldPos);
                    _oldList = tmp;
                }
                for (int i = 0; i < _pos; i++)
                {
                    _oldList[_oldPos] = _list[i];
                    _oldPos++;
                }
            }
            tmp = _list;
            _list = _oldList;
            _oldList = tmp;
            _oldLen = 0;
            _pos = _oldPos;
        }

        private void Grow()
        {
            if (_pos + 1 < _list.Length)
                return;

            FrameLoopItem[] tmp = new FrameLoopItem[_list.Length * 2];
            Array.Copy(_list, tmp, _pos);
            _list = tmp;
        }

        public void Clean()
        {
            _pos = 0;
            _oldPos = 0;
            _oldLen = 0;
        }
    }

}