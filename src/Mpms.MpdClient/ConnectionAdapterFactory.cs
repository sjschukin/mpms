using Microsoft.Extensions.DependencyInjection;
using Mpms.Common;
using Mpms.MpdClient.Base;

namespace Mpms.MpdClient;

public class ConnectionAdapterFactory : IConnectionAdapterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ConnectionAdapterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IConnectionAdapter GetConnectionAdapter(string type)
    {
        return type switch
        {
            _ when type.Equals("UnixSocket", StringComparison.InvariantCultureIgnoreCase) =>
                _serviceProvider.GetService<UnixSocketAdapter>() ??
                throw new InvalidOperationException($"Cannot get {nameof(UnixSocketAdapter)} service."),
            _ when type.Equals("Network", StringComparison.InvariantCultureIgnoreCase) =>
                _serviceProvider.GetService<TcpIpAdapter>() ??
                throw new InvalidOperationException($"Cannot get {nameof(TcpIpAdapter)} service."),
            _ => throw new Exception($"'{type}' connection type is not recognized. Supported: UnixSocket, Network.")
        };
    }
}