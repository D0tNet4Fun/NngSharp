using NngSharp.Native;

namespace NngSharp.Async
{
    public class AsyncOptions
    {
        private readonly NngAio _nngAio;
        private int _timeoutInMilliseconds;

        public AsyncOptions(NngAio nngAio)
        {
            _nngAio = nngAio;
        }

        public int TimeoutInMilliseconds
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