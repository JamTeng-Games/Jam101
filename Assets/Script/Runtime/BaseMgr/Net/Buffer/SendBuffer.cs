using System;
using System.Text;
using UnityEngine;

namespace Jam.Runtime.Net_
{

    public class SendBuffer : NetBuffer
    {
        public SendBuffer(int size) : base(size)
        {
        }

        public int GetReadBuffer(out Span<byte> buffer)
        {
            int readSize = GetReadSize();
            buffer = new Span<byte>(_buffer, _beginIndex, readSize);
            return readSize;
        }

        public void AddPacket(Packet packetToAdd)
        {
            int dataLength = packetToAdd.GetDataLength();
            int packetHeadSize = PacketHead.Size;
            int totalSize = dataLength + packetHeadSize + sizeof(ushort);

            // 长度不够，扩容
            while (GetEmptySize() < totalSize)
            {
                ReAllocBuffer();
            }

            // TODO: 用 CopyFromSpan 代替 CopyFrom

            // Unsafe.As<byte, int>(ref tempBytes[0]) = totalSize;
            // 1.整体长度
            byte[] tempBytes = ByteArrayPool.Rent(sizeof(ushort));
            // 不包括头部的长度
            ushort sizeWithoutLenHead = (ushort)(totalSize - 2);
            // 直接写入大端序
            tempBytes[0] = (byte)((sizeWithoutLenHead >> 8) & 0xFF); // 高位字节
            tempBytes[1] = (byte)(sizeWithoutLenHead & 0xFF);        // 低位字节
            // unsafe
            // {
            //     fixed (byte* pData = tempBytes)
            //     {
            //         *((ushort*)pData) = (ushort)totalSize;
            //     }
            //     // 转成大端序
            //     if (BitConverter.IsLittleEndian)
            //     {
            //         Array.Reverse(tempBytes);
            //     }
            // }

            CopyFrom(tempBytes, sizeof(ushort));
            ByteArrayPool.Return(tempBytes, true);

            // 2.头部
            // tempBytes = ByteArrayPool.Rent(packetHeadSize);
            byte[] tempBytes2 = ByteArrayPool.Rent(packetHeadSize);
            PacketHead head = new PacketHead() { cmdId = (ushort)packetToAdd.CmdId };
            CopyFrom(head.ToBytes(ref tempBytes2), packetHeadSize);
            // ByteArrayPool.Return(tempBytes, true);
            ByteArrayPool.Return(tempBytes2, true);

            // 3.数据
            CopyFrom(packetToAdd.GetBuffer(), packetToAdd.GetDataLength());

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < totalSize; i++)
            {
                sb.Append(_buffer[i] + " ");
            }
            Debug.Log($"PacketSend sz: {_endIndex - _beginIndex}, c: {sb}");
        }

        /// 将 source 拷贝到 _buffer
        private void CopyFrom(byte[] source, int size)
        {
            int writeSize = GetWriteSize();
            if (writeSize < size)
            {
                // // 1.copy到尾部
                // Array.Copy(source, 0, _buffer, _endIndex, writeSize);
                //
                // // 2.copy到头部
                // Array.Copy(source, writeSize, _buffer, 0, size - writeSize);

                // 1.copy到尾部
                System.Buffer.BlockCopy(source, 0, _buffer, _endIndex, writeSize);

                // 2.copy到头部
                System.Buffer.BlockCopy(source, writeSize, _buffer, 0, size - writeSize);
            }
            else
            {
                // Array.Copy(source, 0, _buffer, _endIndex, size);
                System.Buffer.BlockCopy(source, 0, _buffer, _endIndex, size);
            }
            FillData(size);
        }

        private void CopyFromSpan(Span<byte> source, int size)
        {
            int writeSize = GetWriteSize();
            if (writeSize < size)
            {
                // 1.copy到尾部
                Span<byte> tailBuffer = new Span<byte>(_buffer, _endIndex, writeSize);
                source.Slice(0, writeSize).CopyTo(tailBuffer);

                // 2.copy到头部
                Span<byte> headBuffer = new Span<byte>(_buffer, 0, size - writeSize);
                source.Slice(writeSize, size - writeSize).CopyTo(headBuffer);
            }
            else
            {
                Span<byte> dstBuffer = new Span<byte>(_buffer, _endIndex, size);
                source.Slice(0, size).CopyTo(dstBuffer);
            }
            FillData(size);
        }
    }

}