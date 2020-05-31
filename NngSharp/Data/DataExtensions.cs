using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

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
            WriteToMemoryStream(memory, stream =>
            {
                serializer.WriteObject(stream, value);
            });
        }

        public static void SetJson<T>(this IMemory memory, T value, JsonSerializerOptions jsonSerializerOptions = null)
        {
            if (jsonSerializerOptions == null) jsonSerializerOptions = GetDefaultJsonSerializerOptions();
            WriteToMemoryStream(memory, stream =>
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    JsonSerializer.Serialize(writer, value, jsonSerializerOptions);
                }
            });
        }

        private static void WriteToMemoryStream(IMemory memory, Action<Stream> write)
        {
            var capacity = 4; // todo
            memory.Allocate(capacity);
            while (true)
            {
                using (var stream = memory.GetWriteStream())
                {
                    try
                    {
                        write(stream);
                        memory.Length = checked((int)stream.Position);
                        break;
                    }
                    catch (NotSupportedException)
                    {
                        memory.Allocate(capacity *= 2);
                    }
                }
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

        public static T GetJson<T>(this IMemory memory, JsonSerializerOptions jsonSerializerOptions = null)
        {
            if (jsonSerializerOptions == null) jsonSerializerOptions = GetDefaultJsonSerializerOptions();
            var reader = new Utf8JsonReader(memory.AsSpan());
            return JsonSerializer.Deserialize<T>(ref reader, jsonSerializerOptions);
        }

        public static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            return jsonSerializerOptions;
        }
    }
}