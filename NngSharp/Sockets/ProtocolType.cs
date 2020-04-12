namespace NngSharp.Sockets
{
    public enum ProtocolType
    {
        None,
        Pair0 = 16,
        Pair1 = 17,
        Publish = 32,
        Subscribe = 33,
        Push = 80,
        Pull = 81,
        Request = 48,
        Reply = 49,
        Survey = 98,
        Respondent = 99,
        Bus = 112,
    }
}