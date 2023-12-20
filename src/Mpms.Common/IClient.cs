namespace Mpms.Common;

public interface IClient : IDisposable
{
    Task EstablishConnectionAsync(CancellationToken cancellationToken);

    Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request, Func<byte[], TResponse> creator, CancellationToken cancellationToken)
        where TRequest : IRequest
        where TResponse : IResponse;

    Task CloseConnectionAsync(CancellationToken cancellationToken);
}