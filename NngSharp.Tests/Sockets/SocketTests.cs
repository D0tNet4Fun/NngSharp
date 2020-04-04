using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NngSharp.Data;
using NngSharp.Sockets;
using Xunit;
using Buffer = NngSharp.Data.Buffer;

namespace NngSharp.Tests.Sockets
{
    public class SocketTests
    {
        [Fact]
        public void Send_And_Receive_String()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var buffer = new Buffer();
            buffer.SetString("hello!");
            client.Send(buffer);
            
            var buffer2 = server.Receive();
            buffer2.GetString().Should().Be("hello!");
        }

        [Fact]
        public void Send_And_Receive_Struct()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var @struct = new Point {x = 1, y = 2, z = 3};
            var buffer = new Buffer();
            buffer.SetStruct(@struct);
            client.Send(buffer);
            
            var buffer2 = server.Receive();
            var result = buffer2.GetStruct<Point>();

            Assert.Equal(@struct, result);
        }

        [Fact]
        public async Task TrySend_And_TryReceive_String()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var buffer = new Buffer();
            buffer.SetString("hello!");
            var sent = client.TrySend(buffer);
            sent.Should().BeTrue();

            await Task.Delay(1000);
            var received = server.TryReceive(out var buffer2);
            received.Should().BeTrue();
            buffer2.GetString().Should().Be("hello!");
        }

        [Fact]
        public void SendZeroCopy_And_ReceiveZeroCopy()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var encoding = Encoding.UTF8;

            using (var buffer = new ZeroCopyBuffer())
            {
                buffer.SetString("hello!");
                client.SendZeroCopy(buffer);
                Assert.Equal(0, buffer.Length);
            }

            using (var serverBuffer = server.ReceiveZeroCopy())
            {
                Assert.Equal("hello!", serverBuffer.GetString(encoding));
                serverBuffer.SetString("hi!", encoding);
                server.SendZeroCopy(serverBuffer);
            }

            using (var buffer = client.ReceiveZeroCopy())
            {
                Assert.Equal("hi!", buffer.GetString(encoding));
            }
        }

        [Fact]
        public async Task TrySendZeroCopy_And_TryReceiveZeroCopy()
        {
            using var server = new PairSocket();
            using var client = new PairSocket();
            var url = "inproc://here";
            server.Listen(url);
            client.Dial(url);

            var encoding = Encoding.UTF8;
            bool sent, received;

            using (var buffer = new ZeroCopyBuffer())
            {
                buffer.SetString("hello!");
                sent = client.TrySendZeroCopy(buffer);
                buffer.Length.Should().Be(0);
                sent.Should().BeTrue();
            }

            await Task.Delay(1000);
            received = server.TryReceiveZeroCopy(out var serverBuffer);
            received.Should().BeTrue();
            using (serverBuffer)
            {
                serverBuffer.GetString(encoding).Should().Be("hello!");
                serverBuffer.SetString("hi!", encoding);
                sent = server.TrySendZeroCopy(serverBuffer);
                sent.Should().BeTrue();
            }
            
            await Task.Delay(1000);
            received = client.TryReceiveZeroCopy(out var clientBuffer);
            received.Should().BeTrue();
            using(clientBuffer)
            {
                clientBuffer.GetString(encoding).Should().Be("hi!");
            }
        }

        [Fact]
        public void SendMessage_And_ReceiveMessage_String()
        {
            // arrange
            var url = "inproc://here";
            using var server = new PairSocket();
            server.Listen(url);
            using var client = new PairSocket();
            client.Dial(url);

            var message1 = new Message();
            message1.SetString("hello world", Encoding.ASCII);

            // act
            client.SendMessage(message1);
            var message2 = server.ReceiveMessage();

            // assert
            message2.Length.Should().Be(11);
            message2.GetString(Encoding.ASCII).Should().Be("hello world");
        }

        [Fact]
        public void SendMessage_And_ReceiveMessage_Struct()
        {
            // arrange
            var url = "inproc://here";
            using var server = new PairSocket();
            server.Listen(url);
            using var client = new PairSocket();
            client.Dial(url);

            var data = new Point {x = 1, y = 2, z = 3};
            var message1 = new Message();
            message1.SetStruct(data);

            // act
            client.SendMessage(message1);
            var message2 = server.ReceiveMessage();
            var result = message2.GetStruct<Point>();

            // assert
            message2.Length.Should().Be(3 * sizeof(int));
            result.Should().Match<Point>(p => p.x == data.x && p.y == data.y && p.z == data.z);
        }

        //[Fact]
        //public void SendMessage_And_ReceiveMessage_Class()
        //{
        //    // arrange
        //    var url = "inproc://here";
        //    using var server = new PairSocket();
        //    server.Listen(url);
        //    using var client = new PairSocket();
        //    client.Dial(url);

        //    var data = new SmartPoint { x = 1, y = 2, z = 3, t = 4 };
        //    var message1 = new GenericMessage<SmartPoint>(180);
        //    message1.Value = data;

        //    // act
        //    client.SendMessage(message1);
        //    var message2 = server.ReceiveGenericMessage<SmartPoint>();

        //    // assert
        //    Assert.Equal(180, message2.Length);
        //    var receivedData = message2.Value;
        //    Assert.Equal(data, receivedData);
        //}
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Point
    {
        public int x;
        public int y;
        public int z;
    }
}