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
        public static extern NngErrorCode nng_recv(uint socket, IntPtr data, ref UIntPtr size, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_recv(uint socket, out IntPtr data, out UIntPtr size, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_send(uint socket, IntPtr data, UIntPtr size, NngFlags flags);
    }
}