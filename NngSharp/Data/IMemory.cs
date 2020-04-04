using System;
using System.IO;

namespace NngSharp.Data
{
    public interface IMemory
    {
        IntPtr Ptr { get; }
        int Length { get; set; }

        void Allocate(int capacity);

        public unsafe Span<byte> Span => new Span<byte>(Ptr.ToPointer(), Length);

        public unsafe Stream GetReadStream()
        {
            return new UnmanagedMemoryStream((byte*) Ptr.ToPointer(), Length, Length, FileAccess.Read);
        }

        public unsafe Stream GetWriteStream()
        {
            return new UnmanagedMemoryStream((byte*) Ptr.ToPointer(), Length, Length, FileAccess.Write);
        }
    }
}