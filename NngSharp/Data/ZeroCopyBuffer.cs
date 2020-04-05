using System;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class ZeroCopyBuffer : IMemory, IDisposable
    {
        public ZeroCopyBuffer()
        {
        }

        internal ZeroCopyBuffer(IntPtr ptr, int capacity)
        {
            Ptr = ptr;
            Capacity = capacity;
            Length = capacity;
        }

        public void Dispose()
        {
            if (Ptr == IntPtr.Zero) return;

            FreeMemory();
            Clear();
        }

        public IntPtr Ptr { get; private set; }
        public int Length { get; set; }
        public int Capacity { get; private set; }

        public void Allocate(int capacity)
        {
            if (Capacity < capacity)
            {
                // free existing memory and allocate new one
                if (Ptr != IntPtr.Zero) FreeMemory();
                Ptr = NativeMethods.nng_alloc((UIntPtr) capacity);
                Capacity = capacity;
            }
            Length = capacity;
        }

        private void FreeMemory() => NativeMethods.nng_free(Ptr, (UIntPtr)Capacity);

        internal void Clear()
        {
            Ptr = IntPtr.Zero;
            Capacity = 0;
            Length = 0;
        }
    }
}