using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Jam.Core;

namespace Jam.Runtime.Net_
{

    public class Network
    {
        private Socket _socket;
        private RecvBuffer _recvBuffer;
        private SendBuffer _sendBuffer;
        private NetStatus _status;
        private string _ip;
        private int _port;
        protected List<Socket> _readFds;
        protected List<Socket> _writeFds;
        protected List<Socket> _exceptFds;
        private byte[] _testError = new byte[4];

        private Action<NetEvent, int> _eventCallback;
        private Action<Packet> _messageCallback;

        public Network()
        {
            _recvBuffer = new RecvBuffer(Constant.DefaultRecvBufferSize);
            _sendBuffer = new SendBuffer(Constant.DefaultSendBufferSize);
            _readFds = new List<Socket>(1);
            _writeFds = new List<Socket>(1);
            _exceptFds = new List<Socket>(1);
        }

        public void Dispose()
        {
            _socket?.Close();
            _recvBuffer.Dispose();
            _sendBuffer.Dispose();

            _socket = null;
            _recvBuffer = null;
            _sendBuffer = null;
        }

        public void SetEventCallback(Action<NetEvent, int> callback)
        {
            _eventCallback = callback;
        }

        public void SetMessageCallback(Action<Packet> callback)
        {
            _messageCallback = callback;
        }

        public void Tick()
        {
            if (_socket == null)
                return;

            _readFds.Clear();
            _writeFds.Clear();
            _exceptFds.Clear();
            _readFds.Add(_socket);
            _writeFds.Add(_socket);
            _exceptFds.Add(_socket);

            Socket.Select(_readFds, _writeFds, _exceptFds, 0);
            if (_readFds.Contains(_socket))
            {
                OnReadSignal();
            }
            if (_writeFds.Contains(_socket))
            {
                OnWriteSignal();
            }
            if (_exceptFds.Contains(_socket))
            {
                OnErrorSignal();
            }
        }

        private void OnReadSignal()
        {
            if (_status != NetStatus.Connected)
                return;
            if (!Recv())
            {
                _eventCallback?.Invoke(NetEvent.Disconnect, 0);
                Close();
            }
        }

        private void OnWriteSignal()
        {
            if (_status == NetStatus.Connecting)
            {
                try
                {
                    _socket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error, _testError);
                    _status = NetStatus.Connected;
                    _eventCallback?.Invoke(NetEvent.ConnectSuccess, 0);
                }
                catch (Exception e)
                {
                    Close();
                    if (e is SocketException se)
                    {
                        JLog.Error($"connect failed. socket: re connect");
                        _eventCallback?.Invoke(NetEvent.ConnectFailed, (int)se.SocketErrorCode);
                        // 关闭当前socket，重新connect
                        Connect(_ip, _port);
                    }
                    else
                    {
                        JLog.Exception(e);
                        _eventCallback?.Invoke(NetEvent.ConnectFailed, 0);
                    }
                }
                return;
            }

            if (HasSendData())
            {
                Send();
            }
        }

        private void OnErrorSignal()
        {
            JLog.Error($"socket except!! socket {_socket}");
            _eventCallback?.Invoke(NetEvent.Disconnect, 0);
            Close();
        }

        public bool Connect(string ip, int port)
        {
            _ip = ip;
            _port = port;
            _socket = SocketEx.CreateSocket();
            if (_socket == null)
                return false;

            _status = NetStatus.Connecting;
            try
            {
                _socket.Connect(ip, port);
            }
            catch (SocketException e)
            {
                switch (e.SocketErrorCode)
                {
                    // 连接中
                    case SocketError.InProgress:
                    case SocketError.WouldBlock:
                        //
                        break;
                    // 已经连接上
                    case SocketError.Success:
                        _status = NetStatus.Connected;
                        _eventCallback?.Invoke(NetEvent.ConnectSuccess, 0);
                        break;
                    // 连接失败
                    default:
                        JLog.Exception(e);
                        Close();
                        return false;
                }
            }
            catch (Exception e)
            {
                JLog.Exception(e);
                Close();
                return false;
            }

            return true;
        }

        public bool HasRecvData()
        {
            return _recvBuffer.HasData();
        }

        public bool HasSendData()
        {
            return _sendBuffer.HasData();
        }

        public Packet GetRecvPacket()
        {
            return _recvBuffer.GetPacket();
        }

        /// 将Packet放入sendBuffer
        public void SendPacket(Packet packet)
        {
            _sendBuffer.AddPacket(packet);
        }

        private bool Recv()
        {
            bool hasRes = false;
            Span<byte> writeBuf = null;
            while (true)
            {
                // 总空间数据不足一个头的大小，扩容
                if (_recvBuffer.GetEmptySize() < (PacketHead.Size + sizeof(ushort)))
                {
                    _recvBuffer.ReAllocBuffer();
                }

                int writeSize = _recvBuffer.GetWriteBuffer(out writeBuf);
                try
                {
                    int dataSize = _socket.Receive(writeBuf, SocketFlags.None, out SocketError errorCode);
                    if (dataSize > 0)
                    {
                        JLog.Info($"recv size: {dataSize}");
                        _recvBuffer.FillData(dataSize);
                    }
                    else if (dataSize == 0)
                    {
                        if (errorCode == SocketError.Interrupted || errorCode == SocketError.WouldBlock)
                        {
                            hasRes = true;
                            break;
                        }

                        JLog.Warning($"recv size: {dataSize}, error: {errorCode}");
                        break;
                    }
                    else
                    {
                        if (errorCode == SocketError.Interrupted || errorCode == SocketError.WouldBlock)
                        {
                            hasRes = true;
                            break;
                        }

                        JLog.Warning($"recv size: {dataSize}, error: {errorCode}");
                        break;
                    }
                }
                catch (Exception e)
                {
                    if (e is SocketException se)
                    {
                        if (se.SocketErrorCode is SocketError.WouldBlock or SocketError.Interrupted)
                        {
                            hasRes = true;
                        }
                        else
                        {
                            _eventCallback?.Invoke(NetEvent.BadPacket, (int)se.SocketErrorCode);
                            JLog.Exception(e);
                        }
                    }
                    else if (e is ObjectDisposedException ode)
                    {
                        // The Socket has been closed
                        _eventCallback?.Invoke(NetEvent.Disconnect, 0);
                        JLog.Exception(e);
                    }
                    else
                    {
                        _eventCallback?.Invoke(NetEvent.Unknown, 0);
                        JLog.Exception(e);
                    }
                    break;
                }
            }

            if (hasRes)
            {
                while (true)
                {
                    var packet = _recvBuffer.GetPacket();
                    if (packet == null)
                        break;

                    _messageCallback?.Invoke(packet);
                }
            }

            return hasRes;
        }

        /// 发送SendBuffer内的数据
        private bool Send()
        {
            while (true)
            {
                Span<byte> pBuffer = null;
                int needSendSize = _sendBuffer.GetReadBuffer(out pBuffer);

                // 没有数据可发送
                if (needSendSize <= 0)
                    return true;

                try
                {
                    int size = _socket.Send(pBuffer, SocketFlags.None, out SocketError errorCode);
                    if (size > 0)
                    {
                        JLog.Info($"pBuffer size: {pBuffer.Length}");
                        JLog.Info($"send size: {needSendSize}");
                        _sendBuffer.RemoveData(size);

                        // 下一帧再发送
                        if (size < needSendSize)
                            return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    if (e is SocketException se)
                    {
                        if (se.SocketErrorCode == SocketError.Success)
                        {
                            return true;
                        }
                        else
                        {
                            _eventCallback?.Invoke(NetEvent.Unknown, 0);
                            JLog.Exception(se);
                        }
                    }
                    else if (e is ObjectDisposedException ode)
                    {
                        _eventCallback?.Invoke(NetEvent.Disconnect, 0);
                        JLog.Exception(ode);
                    }
                    else
                    {
                        _eventCallback?.Invoke(NetEvent.Unknown, 0);
                        JLog.Exception(e);
                    }

                    return false;
                }
            }
        }

        public void Close()
        {
            if (_status == NetStatus.Closed)
                return;
            _status = NetStatus.Closed;
            _socket.Close();
            _socket = null;
            _recvBuffer.Clear();
            _sendBuffer.Clear();
        }

        public bool IsClosed()
        {
            return _status != NetStatus.Closed;
        }

        public bool IsConnected()
        {
            return _status == NetStatus.Connected;
        }
    }

}