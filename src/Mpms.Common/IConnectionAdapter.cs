namespace Mpms.Common;

public interface IConnectionAdapter : IDisposable, IAsyncDisposable
{
    bool Connected { get; }

    Task<Stream> CreateStreamAsync();
    Task DisconnectAsync();
}