namespace Mpms.Common;

public interface IClient
{
    Task EstablishConnectionAsync();

    Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request)
        where TRequest : IRequest
        where TResponse : IResponse;

    Task CloseConnectionAsync();
}