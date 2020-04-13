using System;
using System.IO;

namespace NngSharp.Data
{
    public interface IMemory
    {
        IntPtr Ptr { get; }
        int Length { get; set; }

        void Allocate(int capacity);
    }
}