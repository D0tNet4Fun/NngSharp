using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_close(uint socket);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern unsafe NngErrorCode nng_recv(uint socket, void* data, ref UIntPtr size, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern unsafe NngErrorCode nng_send(uint socket, void* data, UIntPtr size, NngFlags flags);
    }
}