using System;

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

            // 1.整体长度
            byte[] tempBytes = ByteArrayPool.Rent(sizeof(ushort));
            // Unsafe.As<byte, int>(ref tempBytes[0]) = totalSize;
            unsafe
            {
                fixed (byte* pData = tempBytes)
                {
                    *((int*)pData) = totalSize;
                }
            }

            CopyFrom(tempBytes, sizeof(ushort));
            ByteArrayPool.Return(tempBytes, true);

            // 2.头部
            // tempBytes = ByteArrayPool.Rent(packetHeadSize);
            byte[] tempBytes2 = ByteArrayPool.Rent(packetHeadSize);
            PacketHead head = new PacketHead() { msgId = (ushort)packetToAdd.MsgId };
            CopyFrom(head.ToBytes(ref tempBytes2), packetHeadSize);
            // ByteArrayPool.Return(tempBytes, true);
            ByteArrayPool.Return(tempBytes2, true);

            // 3.数据
            CopyFrom(packetToAdd.GetBuffer(), packetToAdd.GetDataLength());
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