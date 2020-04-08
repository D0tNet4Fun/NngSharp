using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using NngSharp.Sockets.Behaviors;
using Xunit;

namespace NngSharp.Tests.Sockets.Behaviors
{
    public class SocketSendAsyncBehaviorTests : IDisposable
    {
        private readonly Pair0Socket _server;
        private readonly Pair0Socket _client;
        private readonly SocketSendAsyncBehavior _behavior;
        private static readonly string Url = $"inproc://{nameof(SocketSendAsyncBehaviorTests)}";

        public SocketSendAsyncBehaviorTests()
        {
            _server = new Pair0Socket();
            _server.Listen(Url);
            _client = new Pair0Socket();
            _client.Dial(Url);
            _behavior = new SocketSendAsyncBehavior(_client.AsyncContext);
        }

        public void Dispose()
        {
            _server?.Dispose();
            _client?.Dispose();
        }

        private static void FillMemory(IMemory memory) => memory.SetString("hello");

        private void ReceiveAndVerify()
        {
            var buffer = _server.Receive(); // this blocks unless the client sends something, be it Buffer or ZeroCopyBuffer or Message
            buffer.GetString().Should().Be("hello");
        }

        [Fact]
        public async Task SendMessageAsync()
        {
            var message = new Message();
            FillMemory(message);
            await _behavior.SendMessageAsync(message);
            ReceiveAndVerify();
        }

        [Fact]
        public async Task SendMessageAsync_With_Cancellation()
        {
            // kill the server to force client hang while sending
            _server.Dispose();

            // send the message to server
            var message = new Message();
            FillMemory(message);
            using var cts = new CancellationTokenSource();
            var sendMessageAsyncTask = _behavior.SendMessageAsync(message, cts.Token);

            // cancel the operation and wait for it to complete with error
            cts.Cancel();
            Func<Task> act = async () => await sendMessageAsyncTask;
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}