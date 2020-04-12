using System;
using NngSharp.Native;

namespace NngSharp.Async
{
    public class AsyncOptions
    {
        private readonly NngAio _nngAio;
        private TimeSpan _timeoutInMilliseconds = new NativeMethods.NngDuration(NativeMethods.NngDuration.Default).AsTimeSpan();

        public AsyncOptions(NngAio nngAio)
        {
            _nngAio = nngAio;
        }

        public TimeSpan TimeoutInMilliseconds
        {
            get => _timeoutInMilliseconds;
            set
            {
                NativeMethods.nng_aio_set_timeout(_nngAio, new NativeMethods.NngDuration(value));
                _timeoutInMilliseconds = value;
            }
        }
    }
}