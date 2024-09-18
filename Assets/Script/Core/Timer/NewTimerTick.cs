using System;

public class NewTimerTick
{
    private RingList<Action> _nextFrame;

    public NewTimerTick()
    {
        _nextFrame = new RingList<Action>(8);
    }

    public bool NextFrame(Action func)
    {
        return _nextFrame.Set(func);
    }

    public void Tick()
    {
        var lastPos = _nextFrame.WritePos;
        Action func = _nextFrame.Get(lastPos);
        while (func != null)
        {
            func();
            func = _nextFrame.Get(lastPos);
        }
    }
}