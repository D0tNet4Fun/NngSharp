using System.Threading.Tasks;
using NngSharp.Data;

namespace NngSharp.Sockets
{
    public interface IReceiver
    {
        Buffer Receive();
        bool TryReceive(out Buffer buffer);
        ZeroCopyBuffer ReceiveZeroCopy();
        bool TryReceiveZeroCopy(out ZeroCopyBuffer buffer);
        Message ReceiveMessage();
        bool TryReceiveMessage(out Message message);
        Task<Message> ReceiveMessageAsync();
    }
}