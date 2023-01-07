using Microsoft.Extensions.DependencyInjection;
using Mpms.Common;
using Mpms.MpdClient.Base;

namespace Mpms.MpdClient;

public static class MpdClientExtensions
{
    public static IServiceCollection AddMpdClient(this IServiceCollection services)
    {
        return services
            .AddScoped<IConnectionAdapterFactory, ConnectionAdapterFactory>()
            .AddScoped<IClient, MpdClient>()
            .AddScoped<UnixSocketAdapter>()
            .AddScoped<IConnectionAdapter, UnixSocketAdapter>(s => s.GetService<UnixSocketAdapter>()!)
            .AddScoped<TcpIpAdapter>()
            .AddScoped<IConnectionAdapter, TcpIpAdapter>(s => s.GetService<TcpIpAdapter>()!);
    }
}