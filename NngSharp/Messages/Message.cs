using System;
using NngSharp.Native;

namespace NngSharp.Messages
{
    public class Message : IEquatable<Message>, IDisposable
    {
        private NngMsg _nngMessage;

        public Message(int sizeInBytes = 0)
        {
            var errorCode = NativeMethods.nng_msg_alloc(out _nngMessage, (UIntPtr)sizeInBytes);
            ErrorHandler.ThrowIfError(errorCode);
        }

        public Message(NngMsg nngMessage)
        {
            _nngMessage = nngMessage;
        }

        public void Dispose()
        {
            if (_nngMessage.Ptr != default)
            {
                NativeMethods.nng_msg_free(_nngMessage);
                _nngMessage = default;
            }
        }

        public bool Equals(Message other) => _nngMessage.Equals(other?._nngMessage);

        public int Length => _nngMessage.Ptr != default ? (int) NativeMethods.nng_msg_len(_nngMessage) : 0;

        public Span<byte> Body
        {
            get
            {
                if(_nngMessage.Ptr == default) return Span<byte>.Empty;
                var ptr = NativeMethods.nng_msg_body(_nngMessage);
                unsafe
                {
                    return new Span<byte>(ptr.ToPointer(), Length);
                }
            }
        }

        public static implicit operator Span<byte>(Message message) => message.Body;

        public static implicit operator ReadOnlySpan<byte>(Message message) => message.Body;

        public static implicit operator NngMsg(Message message) => message._nngMessage;

        public void Insert(ReadOnlySpan<byte> data) => UpdateBody(data, NativeMethods.nng_msg_insert);

        public void Append(ReadOnlySpan<byte> data) => UpdateBody(data, NativeMethods.nng_msg_append);

        private void UpdateBody(ReadOnlySpan<byte> data, Func<NngMsg, IntPtr, UIntPtr, NngErrorCode> callback)
        {
            NngErrorCode errorCode;
            unsafe
            {
                fixed (byte* ptr = data)
                {
                    errorCode = callback(_nngMessage, (IntPtr)ptr, (UIntPtr)data.Length);
                }
            }
            ErrorHandler.ThrowIfError(errorCode);
        }

        public void Clear() => NativeMethods.nng_msg_clear(_nngMessage);
    }
}