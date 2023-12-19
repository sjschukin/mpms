using Microsoft.Extensions.DependencyInjection;
using Mpms.Mpd.Client.Constants;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMpdClient(this IServiceCollection services)
    {
        return services
            .AddKeyedScoped<IConnectionAdapter, UnixSocketAdapter>(ConnectionConstants.UNIX_SOCKET_TYPE)
            .AddKeyedScoped<IConnectionAdapter, TcpIpAdapter>(ConnectionConstants.NETWORK_TYPE);
    }
}