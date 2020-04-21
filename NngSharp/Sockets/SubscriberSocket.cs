using System;
using System.Runtime.CompilerServices;
using NngSharp.Native;

namespace NngSharp.Sockets
{
    public class SubscriberSocket : SocketBase
    {
        public SubscriberSocket()
            : base(NativeMethods.nng_sub0_open)
        {
        }

        public void SubscribeToAllMessages()
        {
            NativeMethods.nng_socket_set(this, Constants.Options.Protocol.NNG_OPT_SUB_SUBSCRIBE, IntPtr.Zero, UIntPtr.Zero).ThrowIfError();
        }
    }
}