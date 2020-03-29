using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_msg_alloc(out IntPtr message, UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_msg_free(IntPtr message);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_msg_append(IntPtr message, IntPtr value, UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_msg_insert(IntPtr message, IntPtr value, UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_msg_clear(IntPtr message);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern IntPtr nng_msg_body(IntPtr message);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern UIntPtr nng_msg_len(IntPtr message);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_sendmsg(uint socket, IntPtr message, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_recvmsg(uint socket, out IntPtr message, NngFlags flags);
    }
}