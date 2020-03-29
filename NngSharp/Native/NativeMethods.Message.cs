using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_msg_alloc(out NngMsg nngMessage, UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_msg_free(NngMsg nngMessage);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_msg_append(NngMsg nngMessage, IntPtr value, UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_msg_insert(NngMsg nngMessage, IntPtr value, UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern void nng_msg_clear(NngMsg nngMessage);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern IntPtr nng_msg_body(NngMsg nngMessage);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern UIntPtr nng_msg_len(NngMsg nngMessage);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_sendmsg(NngSocket nngSocket, NngMsg nngMessage, NngFlags flags);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_recvmsg(NngSocket nngSocket, out NngMsg nngMessage, NngFlags flags);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NngMsg
    {
        private IntPtr Ptr;
    }
}