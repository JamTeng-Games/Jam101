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

        public void AddMessageHook(NetCmd netCmd, Action<NetCmd, Packet> func)
        {
            _channel.AddMessageHook(netCmd, func);
        }

        public void RemoveMessageHook(NetCmd netCmd, Action<NetCmd, Packet> func)
        {
            _channel.RemoveMessageHook(netCmd, func);
        }

        public void AddGlobalHook(Action<NetCmd, Packet> func)
        {
            _channel.AddGlobalHook(func);
        }

        public void RemoveGlobalHook(Action<NetCmd, Packet> func)
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
        
        public void TestSend(byte[] data)
        {
            _channel.TestSend(data);
        }
    }

}