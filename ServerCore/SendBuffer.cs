using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SendBufferHelper
    {
        static ThreadLocal<SendBuffer> _currentBuffer = new ThreadLocal<SendBuffer>(() => null);
        static int ChunkSize = 4096 * 100;


        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (_currentBuffer.Value == null)
                _currentBuffer.Value = new SendBuffer(ChunkSize);

            if (_currentBuffer.Value.FreeSize < reserveSize)
                _currentBuffer.Value = new SendBuffer(ChunkSize);

            ArraySegment<byte> segment = _currentBuffer.Value.Open(reserveSize);
            Console.WriteLine("fdfdfdfdfd");
            return segment;
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return _currentBuffer.Value.Close(usedSize);
        }
    }
    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize;

        public SendBuffer(int _chunkSize)
        {
            _buffer = new byte[_chunkSize];
        }

        public int FreeSize { get { return _buffer.Length - _usedSize; } }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (FreeSize < reserveSize)
                return null;

            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
        }

        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;
            return segment;
        }
    }
}
