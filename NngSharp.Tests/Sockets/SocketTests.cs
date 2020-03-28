using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class SocketTests
    {

        [Fact]
        public void Send_And_Receive()
        {
            using var sender = new PairSocket();
            using var receiver = new PairSocket();
            var url = "inproc://here";
            sender.Listen(url);
            receiver.Dial(url);
            sender.Send("hello!");
            var result = receiver.Receive();

            Assert.Equal("hello!", result);
        }
    }
}