using System;
using System.Collections.Generic;
using System.Threading;

namespace J.Core
{

    public class ThreadTimer
    {
        private struct ThreadItem
        {
            public bool isRunning;
            public uint id;
            public Action callBack;

            public void Run(object state)
            {
                if (!isRunning && callBack != null)
                {
                    isRunning = true;
                    callBack();
                    isRunning = false;
                }
            }
        }

        private const int DefaultThreadNum = 8;
        private const double Gaptime = 20;

        private Thread _main;
        private uint _id;

        private Action[] _mainList;
        private Action[] _mainOldList;
        private int _mainPos;

        private ThreadItem[] _threadList;
        private List<uint> _mainRemovers;
        private int _threadPos;

        private ThreadItem[] _otherList;
        private int _otherPos;

        private ThreadItem[] _oldList;

        private bool _closed;

        public ThreadTimer()
        {
            ThreadPool.SetMaxThreads(1, 4);

            _id = 0;
            _closed = false;

            _mainList = new Action[DefaultThreadNum];
            // 10毫秒调用一次检查
            _main = NewThread("ThreadMain", ThreadTick);
            _threadList = new ThreadItem[DefaultThreadNum];
            _otherList = new ThreadItem[DefaultThreadNum];
            _oldList = new ThreadItem[DefaultThreadNum];
            _mainRemovers = new List<uint>();
        }

        public void Run(Action startCheckWebConfig)
        {
        }

        public void Destroy()
        {
            _main.Join();
        }

        /// <summary>
        /// 主线程执行
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public void MainFrame(Action callback)
        {
            _mainList[_mainPos] = callback;
            _mainPos++;
        }

        public void ToThread(Action callback)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                callback();
            });
        }

        public void MainTick()
        {
            //_mainList;
        }

        private void ThreadTick()
        {
            while (!_closed)
            {
                var t = DateTime.Now;
                RunMainList();
                double gap = DateTime.Now.Subtract(t).TotalMilliseconds;

                // TODO 其他非threadMain线程触发
                //if (gap < gaptime)
                //{
                //    RunOtherList(true);
                //} else
                //{
                //    RunOtherList(false);
                //}
                //gap = System.DateTime.Now.Subtract(t).TotalMilliseconds;

                if (gap < Gaptime)
                {
                    Thread.Sleep((int)(Gaptime - gap));
                }
            }
        }

        private void RunMainList()
        {
            int pos;
            ThreadItem[] tmp;
            lock (_threadList)
            {
                pos = _threadPos;
                tmp = _oldList;
                _oldList = _threadList;
                _threadList = tmp;
                _threadPos = 0;

                // 执行前删除已删除的函数
                if (_mainRemovers.Count > 0)
                {
                    for (int j = 0; j < _mainRemovers.Count; j++)
                    {
                        var v = _mainRemovers[j];
                        for (int i = 0; i < pos; i++)
                        {
                            if (_oldList[i].id == v)
                            {
                                for (; i < pos - 1; i++)
                                {
                                    _oldList[i] = _oldList[i + 1];
                                }
                                pos--;
                                break;
                                //Array.Copy(_oldlist, _oldlist,)
                            }
                        }
                    }
                    _mainRemovers.Clear();
                }
            }
            // 执行
            for (int i = 0; i < pos; i++)
            {
                _oldList[i].callBack();
            }
            // 执行期间新加入到list,新加入的是少量，把增量加入oldList即可
            lock (_threadList)
            {
                Grow(ref _oldList, pos, _threadPos);
                for (int i = 0; i < _threadPos; i++)
                {
                    _oldList[pos + i] = _threadList[i];
                }
                tmp = _threadList;
                _threadList = _oldList;
                _oldList = tmp;
                _threadPos += pos;
            }
        }

        private void RunOtherList(bool isRunInMain)
        {
            int pos;
            ThreadItem[] tmp;

            lock (_otherList)
            {
                pos = _otherPos;
                tmp = _oldList;
                _oldList = _otherList;
                _otherList = tmp;
                _otherPos = 0;
            }
            if (isRunInMain)
            {
                for (int i = 0; i < pos; i++)
                {
                    _oldList[i].callBack();
                }
            }
            else // 非主线程运行
            {
                for (int i = 0; i < pos; i++)
                {
                    ThreadPool.QueueUserWorkItem(_oldList[i].Run);
                }
            }
            lock (_otherList)
            {
                Grow(ref _oldList, pos, _otherPos);
                for (int i = 0; i < _otherPos; i++)
                {
                    _oldList[pos + i] = _otherList[i];
                }
                tmp = _otherList;
                _otherList = _oldList;
                _oldList = tmp;
                _otherPos += pos;
            }
        }

        public uint AddLoop(Action callback)
        {
            _id++;
            lock (_threadList)
            {
                Grow(ref _threadList, _threadPos);
                _threadList[_threadPos].id = _id;
                _threadList[_threadPos].callBack = callback;
                _threadPos++;
            }
            return _id;
        }

        public void Remove(uint id)
        {
            lock (_threadList)
            {
                for (int i = 0; i < _threadPos; i++)
                {
                    if (_threadList[i].id == id)
                    {
                        for (; i < _threadPos - 1; i++)
                        {
                            _threadList[i] = _threadList[i + 1];
                        }
                        _threadPos--;
                        return;
                    }
                }
                // 执行期间添加
                _mainRemovers.Add(id);
            }
        }

        private void Grow(ref ThreadItem[] list, int curpos, int needNum = 1)
        {
            if (_threadPos + needNum < list.Length)
                return;

            ThreadItem[] tmp = new ThreadItem[list.Length * 2];
            Array.Copy(list, tmp, curpos);
            list = tmp;
        }

        public static Thread NewThread(string name, ThreadStart callback)
        {
            var t = new Thread(callback);
            t.IsBackground = true;
            t.Name = name;
            t.Priority = ThreadPriority.Normal;
            t.Start();
            return t;
        }
    }

}