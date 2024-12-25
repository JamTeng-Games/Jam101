using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Jam.Runtime.Net_
{

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct PacketHead
    {
        [FieldOffset(0)] public ushort cmdId;

        public static readonly int Size = 2;

        public static PacketHead FromBytes(byte[] data)
        {
            PacketHead head = new PacketHead();
            // 大端序
            if (BitConverter.IsLittleEndian)
            {
                (data[0], data[1]) = (data[1], data[0]);
            }
            head.cmdId = BitConverter.ToUInt16(data);
            return head;
        }

        public byte[] ToBytes(ref byte[] data)
        {
            // Unsafe.As<byte, ushort>(ref data[0]) = msgId;
            // return data;

            // 直接写入大端序
            data[0] = (byte)((cmdId >> 8) & 0xFF); // 高位字节
            data[1] = (byte)(cmdId & 0xFF);        // 低位字节

            // unsafe
            // {
            //     // 获取指向data数组的指针
            //     fixed (byte* pData = data)
            //     {
            //         // 将msgId作为ushort写入到data数组的头两个字节
            //         *((ushort*)pData) = msgId;
            //     }
            // }
            return data;
        }
    };

    public class Packet : Buffer
    {
        private int _cmdId;

        public int CmdId => _cmdId;

        public Packet() : this(0)
        {
        }

        public static Packet Create(int cmdId)
        {
            Packet packet = new Packet(cmdId);
            return packet;
        }

        private Packet(int cmdId)
        {
            _cmdId = cmdId;
            CleanBuffer();
            _beginIndex = 0;
            _endIndex = 0;
            _buffer = ByteArrayPool.Rent(Constant.DefaultPacketBufferSize);
            _bufferSize = _buffer.Length;
        }

        public override void Dispose()
        {
            _cmdId = 0;
            base.Dispose();
        }

        public void CleanBuffer()
        {
            if (_buffer != null)
                ByteArrayPool.Return(_buffer, true);
            _beginIndex = 0;
            _endIndex = 0;
            _bufferSize = 0;
        }

        public byte[] GetBuffer()
        {
            return _buffer;
        }

        public int GetDataLength()
        {
            return _endIndex - _beginIndex;
        }

        public void FillData(int size)
        {
            _endIndex += size;
        }

        public void ReAllocBuffer()
        {
            AddAllocBuffer1KB(GetDataLength());
        }

        public void Encode<T>(T obj)
        {
            string s = JsonUtility.ToJson(obj);
            byte[] binData = System.Text.Encoding.UTF8.GetBytes(s);
            while (GetEmptySize() < binData.Length)
            {
                ReAllocBuffer();
            }
            Array.Copy(binData, _buffer, binData.Length);
            FillData(binData.Length);

            // byte[] binData = MemoryPackSerializer.Serialize(obj);
            // // AddBuffer(binData, binData.Length);
            // while (GetEmptySize() < binData.Length)
            // {
            //     ReAllocBuffer();
            // }
            // Array.Copy(binData, _buffer, binData.Length);
            // FillData(binData.Length);
        }

        public T Decode<T>()
        {
            Span<byte> bufferSpan = new Span<byte>(_buffer, 0, GetDataLength());
            string s = System.Text.Encoding.UTF8.GetString(bufferSpan);
            T obj = JsonUtility.FromJson<T>(s);
            return obj;

            // Span<byte> bufferSpan = new Span<byte>(_buffer, 0, GetDataLength());
            // T obj = MemoryPackSerializer.Deserialize<T>(bufferSpan);
            // return obj;
        }

        // private void AddBuffer(byte[] source, int size)
        // {
        //     while (GetEmptySize() < size)
        //     {
        //         ReAllocBuffer();
        //     }
        //     Array.Copy(source, _buffer, size);
        //     FillData(size);
        // }
    }

}