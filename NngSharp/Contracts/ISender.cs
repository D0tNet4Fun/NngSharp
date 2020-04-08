using NngSharp.Data;

namespace NngSharp.Contracts
{
    public interface ISender
    {
        void Send(Buffer buffer);
        bool TrySend(Buffer buffer);
        void SendZeroCopy(ZeroCopyBuffer buffer);
        bool TrySendZeroCopy(ZeroCopyBuffer buffer);
        void SendMessage(Message message);
        bool TrySendMessage(Message message);
    }
}