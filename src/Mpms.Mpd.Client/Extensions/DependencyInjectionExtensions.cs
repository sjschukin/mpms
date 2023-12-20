using Microsoft.Extensions.DependencyInjection;
using Mpms.Common;
using Mpms.Mpd.Client.Constants;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddMpdClient(this IServiceCollection services)
    {
        return services
            .AddKeyedTransient<IConnectionAdapter, UnixSocketAdapter>(ConnectionConstants.UNIX_SOCKET_TYPE)
            .AddKeyedTransient<IConnectionAdapter, TcpIpAdapter>(ConnectionConstants.NETWORK_TYPE)
            .AddTransient<IClient, MpdClient>();
    }
}