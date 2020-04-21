using System.Linq;
using System.Threading.Tasks;
using NngSharp.Data;
using NngSharp.Sockets;
using Xunit;
using Xunit.Abstractions;

namespace NngSharp.Tests.Sockets
{
    public class PubSubSocketTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public PubSubSocketTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Scenario1()
        {
            using var server = new PublisherSocket();
            using var client = new SubscriberSocket();
            var url = "ipc://hello/world";
            server.Listen(url);
            client.Dial(url);
            client.SubscribeToAllMessages();

            var data = Enumerable.Range(0, 10);
            var send = Task.Run(async () =>
            {
                foreach (var x in data)
                {
                    var message = new Message();
                    message.SetStruct(x);
                    _testOutputHelper.WriteLine($"Send: {x}");
                    await server.SendMessageAsync(message);
                }
            });

            var receive = Task.Run(async () =>
            {
                var count = 0;
                while (count++ < 10)
                {
                    var message = await client.ReceiveMessageAsync();
                    var value = message.GetStruct<int>();
                    _testOutputHelper.WriteLine($"Received: {value}");
                }
            });

            await Task.WhenAll(send, receive);
        }
    }
}