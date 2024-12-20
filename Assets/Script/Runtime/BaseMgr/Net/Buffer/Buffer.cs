using System;
using System.Buffers;
using Jam.Core;

namespace Jam.Runtime.Net_
{

    // 环形缓冲区
    public class Buffer
    {
        protected static ArrayPool<byte> ByteArrayPool = ArrayPool<byte>.Shared;

        protected byte[] _buffer;
        protected int _bufferSize = 0;
        protected int _beginIndex = 0;
        protected int _endIndex = 0;

        public int EndIndex => _endIndex;
        public int BeginIndex => _beginIndex;
        public int TotalSize => _bufferSize;

        /// 获取空闲空间大小
        public virtual int GetEmptySize()
        {
            return _bufferSize - _endIndex;
        }

        /// 重新分配缓冲区
        /// dataLength: 现有数据长度
        public void AddAllocBuffer1KB(int dataLength)
        {
            if (_bufferSize >= Constant.MaxBufferSize)
            {
                JLog.Error($"Alloc buffer exceed max size!");
            }

            var temp = ByteArrayPool.Rent((_bufferSize + Constant.AdditionalSize));
            int newEndIndex;
            // 没循环
            if (_beginIndex < _endIndex)
            {
                // Array.Copy(_buffer, _beginIndex, temp, 0, _endIndex - _beginIndex);
                System.Buffer.BlockCopy(_buffer, _beginIndex, temp, 0, _endIndex - _beginIndex);
                newEndIndex = _endIndex - _beginIndex;
            }
            // 有循环
            else
            {
                // 没有数据
                if (_beginIndex == _endIndex && dataLength <= 0)
                {
                    newEndIndex = 0;
                }
                // 有数据 (数据在两端)
                else
                {
                    // 1.先COPY尾部
                    // Array.Copy(_buffer, _beginIndex, temp, 0, _bufferSize - _beginIndex);
                    System.Buffer.BlockCopy(_buffer, _beginIndex, temp, 0, _bufferSize - _beginIndex);
                    newEndIndex = _bufferSize - _beginIndex;

                    // 2.再COPY头部
                    if (_endIndex > 0)
                    {
                        // Array.Copy(_buffer, 0, temp, newEndIndex, _endIndex);
                        System.Buffer.BlockCopy(_buffer, 0, temp, newEndIndex, _endIndex);
                        newEndIndex += _endIndex;
                    }
                }
            }

            // 修改数据
            _bufferSize += Constant.AdditionalSize;
            ByteArrayPool.Return(_buffer, true);
            _buffer = temp;

            _beginIndex = 0;
            _endIndex = newEndIndex;
        }

        public void Clear()
        {
            _bufferSize = 0;
            _beginIndex = 0;
            _endIndex = 0;
        }
        
        public virtual void Dispose()
        {
            if (_buffer != null)
                ByteArrayPool.Return(_buffer);
            _buffer = null;
            _bufferSize = 0;
            _beginIndex = 0;
            _endIndex = 0;
        }
    }

}