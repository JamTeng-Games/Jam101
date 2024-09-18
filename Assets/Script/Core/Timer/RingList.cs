using System;

public class RingList<T>
{
    private T[] _list;
    private uint _rPos;
    private uint _wPos;
    private uint _mask;
    private uint _cap;

    public RingList(int b)
    {
        _cap = (uint)1 << b;
        _list = new T[_cap];
        _rPos = 0;
        _wPos = 0;
        _mask = _cap - 1;
    }

    public bool Set(T func)
    {
        if (_wPos - _rPos >= _cap)
        {
            // var tmp = new T[_cap << 1];
            // if ((_wpos & _mask) > 0)
            // {
            //     var rArrPos = _rpos & _mask;
            //     var wArrPos = _wpos & _mask;
            //     Array.Copy(_list, rArrPos, tmp, 0, _cap - rArrPos);
            //     Array.Copy(_list, 0, tmp, _cap - rArrPos, wArrPos);
            // }
            // return false;

            var tmp = new T[_cap << 1];
            var rArrPos = _rPos & _mask;
            var wArrPos = _wPos & _mask;
            Array.Copy(_list, rArrPos, tmp, 0, _cap - rArrPos);
            Array.Copy(_list, 0, tmp, _cap - rArrPos, wArrPos);
            _list = tmp;

            _rPos = 0;
            _wPos = _cap;
            _cap <<= 1;
            _mask = _cap - 1;
            _list[_wPos & _mask] = func;
            _wPos++;
            return false;
        }
        _list[_wPos & _mask] = func;
        _wPos++;
        return true;
    }

    public uint WritePos => _wPos;

    public T Get(uint lastPos)
    {
        T func = default(T);
        if (_rPos < lastPos)
        {
            func = _list[_rPos & _mask];
            _rPos++;
        }
        return func;
    }

    public T Get()
    {
        T func = default(T);
        if (_rPos < _wPos)
        {
            func = _list[_rPos & _mask];
            _rPos++;
        }
        return func;
    }

    public void Clean()
    {
        _rPos = 0;
        _wPos = 0;
    }
}