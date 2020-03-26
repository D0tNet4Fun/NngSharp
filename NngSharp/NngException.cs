using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using NngSharp.Native;

namespace NngSharp
{
    public class NngException : Exception
    {
        public NngException(NngErrorCode errorCode)
        {
            var ptr = NativeMethods.nng_strerror(errorCode);
            Message = Marshal.PtrToStringAnsi(ptr);
            ErrorCode = errorCode;
        }

        public override string Message { get; }

        public NngErrorCode ErrorCode { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(ErrorCode), ErrorCode);
        }
    }
}