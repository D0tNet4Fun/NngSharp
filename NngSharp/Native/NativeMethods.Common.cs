using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern IntPtr nng_alloc(UIntPtr size);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern IntPtr nng_strerror([MarshalAs(UnmanagedType.I4)] NngErrorCode errorCode);
    }
}