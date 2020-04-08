using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using NngSharp.Sockets.Behaviors;
using Xunit;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Tests.Sockets.Behaviors
{
    public class SocketReceiveAsyncBehaviorTests : IDisposable
    {
        private readonly Pair0Socket _server;
        private readonly Pair0Socket _client;
        private readonly SocketReceiveAsyncBehavior _behavior;
        private static readonly string Url = $"inproc://{nameof(SocketReceiveAsyncBehaviorTests)}";

        public SocketReceiveAsyncBehaviorTests()
        {
            _server = new Pair0Socket();
            _server.Listen(Url);
            _client = new Pair0Socket();
            _client.Dial(Url);
            _behavior = new SocketReceiveAsyncBehavior(_server.AsyncContext);
        }

        public void Dispose()
        {
            _server?.Dispose();
            _client?.Dispose();
        }

        private void Send()
        {
            var buffer = new Buffer();
            buffer.SetString("hello");
            _client.Send(buffer);
        }

        private static void Verify(IMemory memory) => memory.GetString().Should().Be("hello");

        [Fact]
        public async Task ReceiveMessageAsync()
        {
            Send();
            var message = await _behavior.ReceiveMessageAsync();
            Verify(message);
        }

        [Fact]
        public async Task ReceiveMessageAsync_With_Cancellation()
        {
            // do not send a message from client to force the server hang on receive
            //Send();

            using var cts = new CancellationTokenSource();
            var receiveMessageTask = _behavior.ReceiveMessageAsync(cts.Token);

            // cancel receiving and wait for the operation complete as canceled
            cts.Cancel();
            Func<Task> act = async () => await receiveMessageTask;
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}