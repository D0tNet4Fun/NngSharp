using System;
using System.Runtime.CompilerServices;

namespace NngSharp
{
    internal static class ErrorHandler
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfError(NngErrorCode errorCode)
        {
            switch (errorCode)
            {
                case NngErrorCode.Success: return;
                case NngErrorCode.Undefined: throw new InvalidOperationException("Error code was not set by NNG");
                default: throw new NngException(errorCode);
            }
        }
    }
}