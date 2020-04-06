using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NngSharp.Async;
using NngSharp.Data;
using NngSharp.Native;
using NngSharp.Sockets.Behaviors;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Sockets
{
    public class SocketBase : ISender, IReceiver, IAsyncSender, IAsyncReceiver, IDisposable
    {
        private NngSocket _nngSocket;
        private readonly List<NngListener> _listeners = new List<NngListener>();
        private readonly List<NngDialer> _dialers = new List<NngDialer>();

        private readonly SocketSendBehavior _sender;
        private readonly IReceiver _receiver;
        private readonly SocketSendAsyncBehavior _asyncSender;
        private readonly SocketReceiveAsyncBehavior _asyncReceiver;

        protected SocketBase(OpenSocket openSocket)
        {
            var errorCode = openSocket(out _nngSocket);
            ErrorHandler.ThrowIfError(errorCode);

            _sender = new SocketSendBehavior(_nngSocket);
            _receiver = new SocketReceiveBehavior(_nngSocket);

            AsyncContext = new AsyncContext();
            _asyncSender = new SocketSendAsyncBehavior(_nngSocket, AsyncContext);
            _asyncReceiver = new SocketReceiveAsyncBehavior(_nngSocket, AsyncContext);
        }

        public static implicit operator NngSocket(SocketBase socket) => socket._nngSocket;

        public AsyncContext AsyncContext { get; }

        public void Dispose()
        {
            var errorCode = NativeMethods.nng_close(_nngSocket);
            if (errorCode == NngErrorCode.Success)
            {
                _nngSocket = default;
                _listeners.Clear(); // Listeners are implicitly closed when the socket they are associated with is closed
                _dialers.Clear(); // Dialers are implicitly closed when the socket they are associated with is closed.
                return;
            }
            // todo: log error?

            AsyncContext.Dispose();
        }

        public void Listen(string url)
        {
            var errorCode = NativeMethods.nng_listen(_nngSocket, url, out var nngListener, default);
            ErrorHandler.ThrowIfError(errorCode);
            _listeners.Add(nngListener);
        }

        public void Dial(string url)
        {
            var errorCode = NativeMethods.nng_dial(_nngSocket, url, out var nngDialer, default);
            ErrorHandler.ThrowIfError(errorCode);
            _dialers.Add(nngDialer);
        }

        #region Send

        public void Send(Buffer buffer) => _sender.Send(buffer);
        public bool TrySend(Buffer buffer) => _sender.TrySend(buffer);
        public void SendZeroCopy(ZeroCopyBuffer buffer) => _sender.SendZeroCopy(buffer);
        public bool TrySendZeroCopy(ZeroCopyBuffer buffer) => _sender.TrySendZeroCopy(buffer);
        public void SendMessage(Message message) => _sender.SendMessage(message);
        public bool TrySendMessage(Message message) => _sender.TrySendMessage(message);
        public Task SendMessageAsync(Message message) => _asyncSender.SendMessageAsync(message);
        public Task SendMessageAsync(Message message, CancellationToken cancellationToken) => _asyncSender.SendMessageAsync(message, cancellationToken);

        #endregion

        #region Receive

        public Buffer Receive() => _receiver.Receive();
        public bool TryReceive(out Buffer buffer) => _receiver.TryReceive(out buffer);
        public ZeroCopyBuffer ReceiveZeroCopy() => _receiver.ReceiveZeroCopy();
        public bool TryReceiveZeroCopy(out ZeroCopyBuffer buffer) => _receiver.TryReceiveZeroCopy(out buffer);
        public Message ReceiveMessage() => _receiver.ReceiveMessage();
        public bool TryReceiveMessage(out Message message) => _receiver.TryReceiveMessage(out message);
        public Task<Message> ReceiveMessageAsync() => _asyncReceiver.ReceiveMessageAsync();
        public Task<Message> ReceiveMessageAsync(CancellationToken cancellationToken) => _asyncReceiver.ReceiveMessageAsync(cancellationToken);

        #endregion
    }
}

