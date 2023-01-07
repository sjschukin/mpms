using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mpms.Common;

namespace Mpms.MpdClient;

public static class MpdClientExtensions
{
    public static IServiceCollection AddMpdClient(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new MpdConnectionOptions();
        configuration.GetSection(MpdConnectionOptions.SECTION_NAME).Bind(options);

        switch (options.Type)
        {
            case "UnixSocket":
                services.AddTransient<IConnectionAdapter, UnixSocketAdapter>();
                break;
            case "Network":
                services.AddTransient<IConnectionAdapter, TcpIpAdapter>();
                break;
            default:
                throw new Exception("MPD connection type is not recognized. Supported: UnixSocket, Network.");
        }

        services.AddTransient<IClient, MpdClient>();

        return services;
    }
}