namespace Jam.Runtime.Net_
{

    public interface IMsgHandler
    {
        public NetCmd Cmd { get; }
        public void HandleMsg(NetCmd cmd, Packet packet);
    }

}