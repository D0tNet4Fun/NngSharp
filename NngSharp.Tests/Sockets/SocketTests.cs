using System.Text;
using NngSharp.Messages;
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

        [Fact]
        public void SendZeroCopy_And_ReceiveZeroCopy()
        {
            using var sender = new PairSocket();
            using var receiver = new PairSocket();
            var url = "inproc://here";
            sender.Listen(url);
            receiver.Dial(url);
            sender.SendZeroCopy("hello!");
            var result = receiver.ReceiveZeroCopy();

            Assert.Equal("hello!", result);
        }

        [Fact]
        public void SendMessage_And_ReceiveMessage()
        {
            // arrange
            var url = "inproc://here";
            using var server = new PairSocket();
            server.Listen(url);
            using var client = new PairSocket();
            client.Dial(url);

            const string data = "hello world";
            var message1 = new Message(data.Length);
            var encoding = Encoding.UTF8;
            encoding.GetBytes(data, message1);

            // act
            client.SendMessage(message1);
            var message2 = server.ReceiveMessage();

            // assert
            Assert.Equal(11, message2.Length);
            Assert.Equal(message1, message2);
        }
    }
}