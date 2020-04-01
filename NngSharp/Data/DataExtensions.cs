using System.Runtime.InteropServices;
using System.Text;

namespace NngSharp.Data
{
    public static class DataExtensions
    {
        public static void SetString(this IMemory memory, string value) => SetString(memory, value, Encoding.UTF8);

        public static void SetString(this IMemory memory, string value, Encoding encoding)
        {
            var length = encoding.GetByteCount(value);
            memory.Allocate(length);
            encoding.GetBytes(value, memory.Span);
        }

        public static string GetString(this IMemory memory) => GetString(memory, Encoding.UTF8);

        public static string GetString(this IMemory memory, Encoding encoding) => encoding.GetString(memory.Span);

        public static void SetStruct<T>(this IMemory memory, T value)
            where T : struct
        {
            var length = Marshal.SizeOf<T>();
            memory.Allocate(length);
            Marshal.StructureToPtr(value, memory.Ptr, false);
        }

        public static T GetStruct<T>(this IMemory memory)
        {
            return Marshal.PtrToStructure<T>(memory.Ptr);
        }
    }
}