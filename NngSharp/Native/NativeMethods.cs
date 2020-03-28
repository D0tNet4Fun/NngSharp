namespace NngSharp.Native
{
    internal static partial class NativeMethods
    {
        internal enum NngFlags
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