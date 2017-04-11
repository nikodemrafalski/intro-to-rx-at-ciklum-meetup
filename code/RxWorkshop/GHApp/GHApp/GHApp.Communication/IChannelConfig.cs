namespace GHApp.Communication
{
    public interface IChannelConfig
    {
        string Address { get; set; }

        int Port { get; set; }
    }
}