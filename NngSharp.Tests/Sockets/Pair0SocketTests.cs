using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class Pair0SocketTests
    {
        [Fact]
        public void CreatePairSocket()
        {
            using var socket1 = new Pair0Socket();
            using var socket2 = new Pair0Socket();
            var url = "ipc://hello/world";
            socket1.Listen(url);
        }
    }
}