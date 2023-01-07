namespace Mpms.Common;

public interface IConnectionAdapter : IDisposable, IAsyncDisposable
{
    Task<Stream> CreateStreamAsync();
    Task DisconnectAsync();
}