using NngSharp.Sockets;
using Xunit;

namespace NngSharp.Tests.Sockets
{
    public class PairSocketTests
    {
        [Fact]
        public void CreatePairSocket()
        {
            using (var socket = new PairSocket())
            {
            }
        }
    }
}