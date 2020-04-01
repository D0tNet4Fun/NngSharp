using System;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class ZeroCopyBuffer : IMemory, IDisposable
    {
        public ZeroCopyBuffer()
        {
        }

        internal ZeroCopyBuffer(IntPtr ptr, int length)
        {
            Ptr = ptr;
            Capacity = Length = length;
        }

        public void Dispose()
        {
            if (Ptr == IntPtr.Zero) return;

            FreeMemory();
            Clear();
        }

        public IntPtr Ptr { get; private set; }
        public int Length { get; private set; }
        public int Capacity { get; private set; }

        public void Allocate(in int length)
        {
            if (Capacity > length)
            {
                // memory is already allocated
                Length = length;
                return;
            }
            // free existing memory and allocate new one
            if (Ptr != IntPtr.Zero) FreeMemory();
            Ptr = NativeMethods.nng_alloc((UIntPtr)length);
            Capacity = Length = length;
        }

        private void FreeMemory() => NativeMethods.nng_free(Ptr, (UIntPtr)Length);

        internal void Clear()
        {
            Ptr = IntPtr.Zero;
            Capacity = Length =  0;
        }

        public unsafe Span<byte> Span => new Span<byte>(Ptr.ToPointer(), Length);
    }
}