using System;
using NngSharp.Native;

namespace NngSharp.Messages
{
    public class Message : IEquatable<Message>, IDisposable
    {
        private IntPtr _messagePtr;

        public Message(int sizeInBytes = 0)
        {
            var errorCode = NativeMethods.nng_msg_alloc(out _messagePtr, (UIntPtr)sizeInBytes);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public Message(IntPtr ptr)
        {
            _messagePtr = ptr;
        }

        public void Dispose()
        {
            if (_messagePtr != default)
            {
                NativeMethods.nng_msg_free(_messagePtr);
                _messagePtr = default;
            }
        }

        public bool Equals(Message other) => _messagePtr == other?._messagePtr;

        public int Length => (int)NativeMethods.nng_msg_len(_messagePtr);

        public Span<byte> Body
        {
            get
            {
                var ptr = NativeMethods.nng_msg_body(_messagePtr);
                unsafe
                {
                    return new Span<byte>(ptr.ToPointer(), Length);
                }
            }
        }

        public static implicit operator Span<byte>(Message message) => message.Body;

        public static implicit operator ReadOnlySpan<byte>(Message message) => message.Body;

        public static implicit operator IntPtr(Message message) => message._messagePtr;

        public void Insert(ReadOnlySpan<byte> data) => UpdateBody(data, NativeMethods.nng_msg_insert);

        public void Append(ReadOnlySpan<byte> data) => UpdateBody(data, NativeMethods.nng_msg_append);

        private void UpdateBody(ReadOnlySpan<byte> data, Func<IntPtr, IntPtr, UIntPtr, NngErrorCode> callback)
        {
            NngErrorCode errorCode;
            unsafe
            {
                fixed (byte* ptr = data)
                {
                    errorCode = callback(_messagePtr, (IntPtr)ptr, (UIntPtr)data.Length);
                }
            }
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void Clear() => NativeMethods.nng_msg_clear(_messagePtr);
    }
}