using System;
using System.Runtime.InteropServices;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class Message : IMemory, IDisposable
    {
        private NngMsg _nngMessage;
        private bool _isMemoryAllocated;

        public Message()
        {
        }

        internal Message(NngMsg nngMessage)
        {
            _nngMessage = nngMessage;
            _isMemoryAllocated = true;
        }

        public void Dispose()
        {
            if (!_isMemoryAllocated) return;

            NativeMethods.nng_msg_free(_nngMessage);
            _nngMessage = default;
            _isMemoryAllocated = false;
        }

        public int Length => _isMemoryAllocated ? (int) NativeMethods.nng_msg_len(_nngMessage) : 0;

        public IntPtr Ptr => _isMemoryAllocated ? NativeMethods.nng_msg_body(_nngMessage) : IntPtr.Zero;

        public unsafe Span<byte> Span => new Span<byte>(Ptr.ToPointer(), Length);

        public static implicit operator NngMsg(Message message) => message._nngMessage;

        public void Allocate(in int length)
        {
            if(_isMemoryAllocated) throw new NotImplementedException();

            var errorCode = NativeMethods.nng_msg_alloc(out _nngMessage, (UIntPtr)length);
            ErrorHandler.ThrowIfError(errorCode);
            _isMemoryAllocated = true;
        }

        public void Clear()
        {
            NativeMethods.nng_msg_clear(_nngMessage);
        }
    }
}