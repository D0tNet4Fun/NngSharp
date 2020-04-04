using System;
using System.Runtime.CompilerServices;

namespace NngSharp.Data
{
    public class Buffer : IMemory
    {
        private byte[] _bytes;

        public Buffer()
        {
        }

        internal Buffer(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive");
            _bytes = new byte[capacity];
            Length = capacity;
        }

        public unsafe IntPtr Ptr => _bytes != null ? (IntPtr)Unsafe.AsPointer(ref _bytes[0]) : IntPtr.Zero;
        public int Length { get; set; }

        public int Capacity => _bytes?.Length ?? 0;

        public void Allocate(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be positive");
            if (_bytes == null || _bytes.Length < capacity)
            {
                _bytes = new byte[capacity];
            }
            Length = capacity;
        }
    }
}