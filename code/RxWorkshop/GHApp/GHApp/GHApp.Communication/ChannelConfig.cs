namespace GHApp.Communication
{
    public struct ChannelConfig : IChannelConfig
    {
        public string Address { get; set; }

        public int Port { get; set; }
    }
}