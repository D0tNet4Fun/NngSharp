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
            _bytes = new byte[capacity];
            Length = _bytes.Length;
        }

        public unsafe IntPtr Ptr => Capacity == 0 ? IntPtr.Zero : (IntPtr) Unsafe.AsPointer(ref _bytes[0]);
        public int Length { get; set; }
        public int Capacity => _bytes?.Length ?? 0;

        public void Allocate(in int length)
        {
            if (Capacity > length)
            {
                // memory is already allocated
                Length = length;
                return;
            }
            _bytes = new byte[length];
            Length = length;
        }

        public unsafe Span<byte> Span => new Span<byte>(Ptr.ToPointer(), Length);
    }
}