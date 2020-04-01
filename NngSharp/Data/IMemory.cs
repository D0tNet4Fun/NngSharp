using System;

namespace NngSharp.Data
{
    public interface IMemory
    {
        IntPtr Ptr { get; }

        Span<byte> Span { get; }

        void Allocate(in int length);
    }
}