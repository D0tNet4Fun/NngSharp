using System;
using System.IO;

namespace NngSharp.Data
{
    internal static class MemoryExtensions
    {
        public static unsafe Span<byte> AsSpan(this IMemory memory) => new Span<byte>(memory.Ptr.ToPointer(), memory.Length);

        public static unsafe Stream GetReadStream(this IMemory memory)
        {
            return new UnmanagedMemoryStream((byte*)memory.Ptr.ToPointer(), memory.Length, memory.Length, FileAccess.Read);
        }

        public static unsafe Stream GetWriteStream(this IMemory memory)
        {
            return new UnmanagedMemoryStream((byte*)memory.Ptr.ToPointer(), memory.Length, memory.Length, FileAccess.Write);
        }
    }
}