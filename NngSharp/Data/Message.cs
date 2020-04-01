using System;
using System.Runtime.InteropServices;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class Message : IEquatable<Message>, IDisposable
    {
        private NngMsg _nngMessage;
        private bool _isDisposed;

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
            if (_isDisposed) return;

            NativeMethods.nng_msg_free(_nngMessage);
            _nngMessage = default;
            _isDisposed = true;
        }

        public bool Equals(Message other) => _nngMessage.Equals(other?._nngMessage);

        public int Length => _isDisposed ? 0 : (int)NativeMethods.nng_msg_len(_nngMessage);

        public IntPtr BodyPtr => NativeMethods.nng_msg_body(_nngMessage);

        public Span<byte> Body
        {
            get
            {
                if (_isDisposed) return Span<byte>.Empty;
                unsafe
                {
                    return new Span<byte>(BodyPtr.ToPointer(), Length);
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

    public class Message<T> : Message
        where T : struct
    {
        public Message(T value)
            : base(Marshal.SizeOf(value))
        {
            Value = value;
        }

        public Message(NngMsg nngMessage)
            : base(nngMessage)
        {
        }

        public T Value
        {
            get => Marshal.PtrToStructure<T>(BodyPtr);
            set => Marshal.StructureToPtr(value, BodyPtr, false);
        }
    }
}