namespace Mpms.Mpd.Common;

public interface IConnectionAdapter : IDisposable, IAsyncDisposable
{
    bool IsConnected { get; }

    Task<Stream> CreateStreamAsync();
    Task DisconnectAsync();
}