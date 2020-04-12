using System;
using System.Runtime.InteropServices;

namespace NngSharp.Native
{
    internal static partial class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct NngDuration
        {
            public const int Zero = 0;
            public const int Infinite = -1;
            public const int Default = -2;

            public int Milliseconds;

            public NngDuration(int milliseconds)
                : this()
            {
                Milliseconds = milliseconds;
            }

            public NngDuration(TimeSpan timeSpan)
                : this(checked((int)timeSpan.TotalMilliseconds))
            {

            }

            public TimeSpan AsTimeSpan() => TimeSpan.FromMilliseconds(Milliseconds);
        }

        [Flags]
        public enum NngFlags
        {
            None = 0,
            /// <summary>
            /// NNG_FLAG_ALLOC
            /// </summary>
            Allocate = 1, // Recv to allocate receive buffer.
            /// <summary>
            /// NNG_FLAG_NONBLOCK
            /// </summary>
            Async = 2  // Non-blocking operations.
        }
    }
}