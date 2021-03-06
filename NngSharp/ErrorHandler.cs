﻿using System;
using System.Runtime.CompilerServices;

namespace NngSharp
{
    internal static class ErrorHandler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfError(this NngErrorCode errorCode)
        {
            switch (errorCode)
            {
                case NngErrorCode.Success: return;
                case NngErrorCode.Unknown: throw new InvalidOperationException("Error code was not set by NNG");
                case NngErrorCode.TimedOut: throw new TimeoutException();
                default: throw new NngException(errorCode);
            }
        }
    }
}