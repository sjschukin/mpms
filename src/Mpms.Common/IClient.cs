namespace Mpms.Common;

public interface IClient : IDisposable, IAsyncDisposable
{
    string? Name { get; }
    string? ProtocolVersion { get; }
    bool IsConnectionEstablished { get; }

    Task EstablishConnectionAsync();

    Task<TResponse> SendRequestAsync<TResponse>(IRequest request)
        where TResponse : IResponse, new();

    Task CloseConnectionAsync();
}