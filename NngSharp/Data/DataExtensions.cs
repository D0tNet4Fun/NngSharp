using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
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
#if !NETFRAMEWORK
            encoding.GetBytes(value, memory.AsSpan());
#else
            unsafe
            {
                fixed (char* chars = value)
                {
                    encoding.GetBytes(chars, value.Length, (byte*)memory.Ptr, length);
                }
            }
#endif
        }

        public static string GetString(this IMemory memory) => GetString(memory, Encoding.UTF8);

        public static string GetString(this IMemory memory, Encoding encoding)
        {
#if !NETFRAMEWORK
            return encoding.GetString(memory.AsSpan());
#else
            unsafe
            {
                return encoding.GetString((byte*)memory.Ptr, memory.Length);
            }
#endif
        }

        public static void SetStruct<T>(this IMemory memory, T value)
            where T : struct
        {
            var length = Marshal.SizeOf<T>();
            memory.Allocate(length);
            Marshal.StructureToPtr(value, memory.Ptr, false);
        }

        public static T GetStruct<T>(this IMemory memory)
            where T : struct
        {
            return Marshal.PtrToStructure<T>(memory.Ptr);
        }

        public static void SetDataContract<T>(this IMemory memory, T value) => SetDataContract(memory, value, new DataContractJsonSerializer(typeof(T)));

        public static void SetDataContract<T>(this IMemory memory, T value, DataContractJsonSerializer serializer)
        {
            var capacity = 128; // todo
            memory.Allocate(capacity);
            while (true)
            {
                using (var stream = memory.GetWriteStream())
                {
                    try
                    {
                        serializer.WriteObject(stream, value);
                    }
                    catch (NotSupportedException)
                    {
                        memory.Allocate(capacity *= 2);
                        continue;
                    }
                    memory.Length = checked((int)stream.Position);
                }

                break;
            }
        }

        public static T GetDataContract<T>(this IMemory memory) => GetDataContract<T>(memory, new DataContractJsonSerializer(typeof(T)));

        public static T GetDataContract<T>(this IMemory memory, DataContractJsonSerializer serializer)
        {
            using (var stream = memory.GetReadStream())
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}