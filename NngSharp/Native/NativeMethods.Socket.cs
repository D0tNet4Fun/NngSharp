﻿using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    using static Constants;

    internal static partial class NativeMethods
    {
        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_close(uint socket);

        [DllImport(NngDll, CallingConvention = NngCallingConvention)]
        public static extern NngErrorCode nng_pair0_open(out uint socket);
    }
}