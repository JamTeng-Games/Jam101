using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class NetMgr : IMgr
    {
        private NetChannel _channel;

        public void Init()
        {
            _channel = new NetChannel(1);
        }

        public void Shutdown(bool isAppQuit)
        {
            _channel.Dispose();
        }

        public void Tick(float dt)
        {
            // _network.Tick();
            _channel.Tick();
        }

        public void Connect(string ip, int port, System.Action<bool> callback)
        {
            _channel.Connect(ip, port, callback);
        }

        public void Close()
        {
            _channel.Close();
        }

        public void Send(Packet packet)
        {
            _channel.Send(packet);
        }

        public bool IsClosed()
        {
            return _channel.IsClosed();
        }

        public bool IsConnected()
        {
            return _channel.IsConnected();
        }

        public void AddMessageHook(MsgId msgId, Action<MsgId, Packet> func)
        {
            _channel.AddMessageHook(msgId, func);
        }

        public void RemoveMessageHook(MsgId msgId, Action<MsgId, Packet> func)
        {
            _channel.RemoveMessageHook(msgId, func);
        }

        public void AddGlobalHook(Action<MsgId, Packet> func)
        {
            _channel.AddGlobalHook(func);
        }

        public void RemoveGlobalHook(Action<MsgId, Packet> func)
        {
            _channel.RemoveGlobalHook(func);
        }

        public void AddEventHook(Action<NetEvent, int> func)
        {
            _channel.AddEventHook(func);
        }

        public void RemoveEventHook(Action<NetEvent, int> func)
        {
            _channel.RemoveEventHook(func);
        }
    }

}