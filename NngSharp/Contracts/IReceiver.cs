using NngSharp.Data;

namespace NngSharp.Contracts
{
    public interface IReceiver
    {
        Buffer Receive();
        bool TryReceive(out Buffer buffer);
        ZeroCopyBuffer ReceiveZeroCopy();
        bool TryReceiveZeroCopy(out ZeroCopyBuffer buffer);
        Message ReceiveMessage();
        bool TryReceiveMessage(out Message message);
    }
}