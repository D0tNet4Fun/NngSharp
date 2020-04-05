using System;
using NngSharp.Native;

namespace NngSharp.Data
{
    public class Message : IMemory, IDisposable
    {
        private NngMsg _nngMessage;
        private int _length;

        public Message()
        {
        }

        internal Message(NngMsg nngMessage)
        {
            _nngMessage = nngMessage;
            UpdateState();
        }

        public void Dispose()
        {
            if (Capacity == 0) return;

            NativeMethods.nng_msg_free(_nngMessage);
            _nngMessage = default;
            Ptr = IntPtr.Zero;
            Capacity = 0;
            _length = 0;
        }

        public int Capacity { get; private set; }

        public IntPtr Ptr { get; private set; }

        public int Length
        {
            get => _length;
            set
            {
                var toChop = Capacity - value;
                if (toChop > 0)
                {
                    var errorCode = NativeMethods.nng_msg_chop(_nngMessage, (UIntPtr)toChop);
                    ErrorHandler.ThrowIfError(errorCode);
                }
                _length = value;
            }
        }

        public static implicit operator NngMsg(Message message) => message._nngMessage;

        public void Allocate(int capacity)
        {
            // either allocate new memory or reallocate more memory
            
            if (Capacity == 0)
            {
                var errorCode = NativeMethods.nng_msg_alloc(out _nngMessage, (UIntPtr)capacity);
                ErrorHandler.ThrowIfError(errorCode);
                UpdateState();
                return;
            }

            if (Capacity < capacity)
            {
                var errorCode = NativeMethods.nng_msg_realloc(_nngMessage, (UIntPtr)capacity);
                ErrorHandler.ThrowIfError(errorCode);
                UpdateState();
            }
            else
            {
                Length = capacity;
            }
        }

        public void Clear()
        {
            NativeMethods.nng_msg_clear(_nngMessage);
            UpdateState();
        }

        private void UpdateState()
        {
            Ptr = NativeMethods.nng_msg_body(_nngMessage);
            _length = (int)NativeMethods.nng_msg_len(_nngMessage);
            Capacity = _length;
        }
    }
}