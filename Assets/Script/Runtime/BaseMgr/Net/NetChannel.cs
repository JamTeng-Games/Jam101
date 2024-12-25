using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Jam.Core;

namespace Jam.Runtime.Net_
{

    public enum NetStatus
    {
        Closed,
        Connecting,
        Connected,
    }

    public class NetChannel
    {
        // Network
        private Network _net;

        private int _recvBufSize;
        private Dictionary<NetCmd, List<Action<NetCmd, Packet>>> _messageHooks;
        private List<Action<NetCmd, Packet>> _globalHooks;
        private List<Action<NetEvent, int>> _eventHooks;
        private Action<bool> _connectCallback;

        public NetChannel(int recvBufSize)
        {
            _net = new Network();
            _recvBufSize = recvBufSize;
            _messageHooks = new Dictionary<NetCmd, List<Action<NetCmd, Packet>>>();
            _globalHooks = new List<Action<NetCmd, Packet>>();
            _eventHooks = new List<Action<NetEvent, int>>();

            _net.SetEventCallback(OnNetEvent);
            _net.SetMessageCallback(OnNetMessage);
        }

        public void Dispose()
        {
            _net.Dispose();
        }

        public void Tick()
        {
            _net.Tick();
        }

        public void Close()
        {
            if (IsClosed())
            {
                JLog.Warning($"NetChannel already closed");
                return;
            }
            _net.Close();
        }

        public bool IsClosed()
        {
            return !_net.IsClosed();
        }

        public bool IsConnected()
        {
            return _net.IsConnected();
        }

        public void Connect(string ip, int port, Action<bool> callback)
        {
            // 必须处于关闭状态才能连接
            if (!IsClosed())
            {
                callback?.Invoke(false);
                return;
            }

            _connectCallback = callback;
            // 连接
            var success = _net.Connect(ip, port);
            // 连接失败
            if (!success)
            {
                JLog.Error($"NetChannel connect to server({ip}:{port}) failed.");
                callback?.Invoke(false);
                return;
            }
        }

        public void Send(Packet packet)
        {
            if (!IsConnected())
            {
                JLog.Warning($"NetChannel not connected");
                return;
            }
            // if (packet.MsgId != (int)NetCmd.KeepAlive)
            // {
            //     JLog.Debug($"Send msgId: {packet.MsgId}");
            // }

            _net.SendPacket(packet);
        }

        private void OnNetEvent(NetEvent evt, int code)
        {
            // 连接成功
            if (evt == NetEvent.ConnectSuccess)
            {
                _connectCallback?.Invoke(true);
                _connectCallback = null;
            }
            // 断开连接(出错导致)
            else if (evt == NetEvent.ConnectFailed || evt == NetEvent.Disconnect)
            {
                _connectCallback?.Invoke(false);
                _connectCallback = null;
                Close();
            }

            // 分发事件
            foreach (var func in _eventHooks)
            {
                func(evt, code);
            }
        }

        private void OnNetMessage(Packet packet)
        {
            NetCmd netCmd = (NetCmd)packet.CmdId;
            // 分发消息Hook
            if (_messageHooks.TryGetValue(netCmd, out var msgHooks))
            {
                foreach (var func in msgHooks)
                {
                    func(netCmd, packet);
                }
            }

            // 分发全局Hook
            foreach (var func in _globalHooks)
            {
                func(netCmd, packet);
            }
        }

        public void AddMessageHook(NetCmd netCmd, Action<NetCmd, Packet> func)
        {
            if (!_messageHooks.TryGetValue(netCmd, out var msgHooks))
            {
                msgHooks = new List<Action<NetCmd, Packet>>();
                _messageHooks.Add(netCmd, msgHooks);
            }
            msgHooks.Add(func);
        }

        public void RemoveMessageHook(NetCmd netCmd, Action<NetCmd, Packet> func)
        {
            if (_messageHooks.TryGetValue(netCmd, out var msgHooks))
            {
                for (int i = msgHooks.Count - 1; i >= 0; i--)
                {
                    if (Equals(msgHooks[i], func))
                    {
                        msgHooks.RemoveAt(i);
                        return;
                    }
                }
            }
            JLog.Warning($"NetChannel RemoveMessageHook failed, not found cmdId: {netCmd}");
        }

        public void AddGlobalHook(Action<NetCmd, Packet> func)
        {
            foreach (var v in _globalHooks)
            {
                if (Equals(v, func))
                {
                    JLog.Warning("NetChannel AddGlobalHook failed, already exists");
                    return;
                }
            }
            _globalHooks.Add(func);
        }

        public void RemoveGlobalHook(Action<NetCmd, Packet> func)
        {
            for (int i = _globalHooks.Count - 1; i >= 0; i--)
            {
                if (Equals(_globalHooks[i], func))
                {
                    _globalHooks.RemoveAt(i);
                    return;
                }
            }
            JLog.Warning("NetChannel RemoveGlobalHook failed, not found");
        }

        public void AddEventHook(Action<NetEvent, int> func)
        {
            foreach (var v in _eventHooks)
            {
                if (Equals(v, func))
                {
                    JLog.Warning("NetChannel AddEventHook failed, already exists");
                    return;
                }
            }
            _eventHooks.Add(func);
        }

        public void RemoveEventHook(Action<NetEvent, int> func)
        {
            for (int i = _eventHooks.Count - 1; i >= 0; i--)
            {
                if (Equals(_eventHooks[i], func))
                {
                    _eventHooks.RemoveAt(i);
                    return;
                }
            }
            JLog.Warning("NetChannel RemoveEventHook failed, not found");
        }

        public void TestSend(byte[] data)
        {
            _net.TestSend(data);
        }
    }

}