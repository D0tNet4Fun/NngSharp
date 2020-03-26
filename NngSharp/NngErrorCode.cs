namespace NngSharp
{
    public enum NngErrorCode
    {
        Unknown = -1,
        Success,
        /// <summary>
        /// NNG_EINTR
        /// </summary>
        Interrupted = 1,
        /// <summary>
        /// NNG_ENOMEM
        /// </summary>
        NoMemory = 2,
        /// <summary>
        /// NNG_EINVAL
        /// </summary>
        InvalidArgument = 3,
        /// <summary>
        /// NNG_EBUSY
        /// </summary>
        ResourceBusy = 4,
        /// <summary>
        /// NNG_ETIMEDOUT
        /// </summary>
        TimedOut = 5,
        /// <summary>
        /// NNG_ECONNREFUSED
        /// </summary>
        ConnectionRefused = 6,
        /// <summary>
        /// NNG_ECLOSED
        /// </summary>
        Closed = 7,
        /// <summary>
        /// NNG_EAGAIN
        /// </summary>
        TryAgain = 8,
        /// <summary>
        /// NNG_ENOTSUP
        /// </summary>
        NotSupported = 9,
        /// <summary>
        /// NNG_EADDRINUSE
        /// </summary>
        AddressInUse = 10,
        /// <summary>
        /// NNG_ESTATE
        /// </summary>
        IncorrectState = 11,
        /// <summary>
        /// NNG_ENOENT
        /// </summary>
        NoEntry = 12,
        /// <summary>
        /// NNG_EPROTO
        /// </summary>
        ProtocolError = 13,
        /// <summary>
        /// NNG_EUNREACHABLE
        /// </summary>
        DestinationUnreachable = 14,
        /// <summary>
        /// NNG_EADDRINVAL
        /// </summary>
        AddressInvalid = 15,
        /// <summary>
        /// NNG_EPERM
        /// </summary>
        PermissionDenied = 16,
        /// <summary>
        /// NNG_EMSGSIZE
        /// </summary>
        MessageSize = 17,
        /// <summary>
        /// NNG_ECONNABORTED
        /// </summary>
        ConnectionAborted = 18,
        /// <summary>
        /// NNG_ECONNRESET
        /// </summary>
        ConnectionReset = 19,
        /// <summary>
        /// NNG_ECANCELED
        /// </summary>
        OperationCanceled = 20,
        /// <summary>
        /// NNG_ENOFILES
        /// </summary>
        NoFiles = 21,
        /// <summary>
        /// NNG_ENOSPC
        /// </summary>
        NoSpace = 22,
        /// <summary>
        /// NNG_EEXIST
        /// </summary>
        ExistsResource = 23,
        /// <summary>
        /// NNG_EREADONLY
        /// </summary>
        ReadOnlyResource = 24,
        /// <summary>
        /// NNG_EWRITEONLY
        /// </summary>
        WriteOnlyResource = 25,
        /// <summary>
        /// NNG_ECRYPTO
        /// </summary>
        CryptographicError = 26,
        /// <summary>
        /// NNG_EPEERAUTH
        /// </summary>
        PeerAuthenticationFailed = 27,
        /// <summary>
        /// NNG_ENOARG
        /// </summary>
        NoArguments = 28,
        /// <summary>
        /// NNG_EAMBIGUOUS
        /// </summary>
        AmbiguousOption = 29,
        /// <summary>
        /// NNG_EBADTYPE
        /// </summary>
        BadType = 30,
        /// <summary>
        /// NNG_ECONNSHUT
        /// </summary>
        ConnectionShutdown = 31,
        /// <summary>
        /// NNG_EINTERNAL
        /// </summary>
        InternalError = 1000,
        /// <summary>
        /// NNG_ESYSERR
        /// </summary>
        SystemError = 0x10000000,
        /// <summary>
        /// NNG_ETRANERR
        /// </summary>
        TransportError = 0x20000000
	}
}