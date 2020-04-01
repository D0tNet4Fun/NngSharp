using System.IO;
using System.Runtime.Serialization;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class GenericMessage<T> : Message
    {
        private readonly DataContractSerializer _serializer;

        public GenericMessage(int sizeInBytes = 0) : base(sizeInBytes)
        {
            _serializer = new DataContractSerializer(typeof(T));
        }

        public GenericMessage(NngMsg nngMessage) : base(nngMessage)
        {
            _serializer = new DataContractSerializer(typeof(T));
        }

        public T Value
        {
            get
            {
                unsafe
                {
                    using var stream = new UnmanagedMemoryStream((byte*) BodyPtr.ToPointer(), Length, Length, FileAccess.Read);
                    return (T)_serializer.ReadObject(stream);
                }
            }
            set
            {
                unsafe
                {
                    using var stream = new UnmanagedMemoryStream((byte*)BodyPtr.ToPointer(), Length, Length, FileAccess.Write);
                    _serializer.WriteObject(stream, value);
                }
            }
        }
    }
}