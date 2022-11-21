namespace Mpms.Protocol;

public class MpdTcpClientParameters
{
    public string Host { get; init; } = "127.0.0.1";
    public int Port { get; init; } = 6600;
    public int Timeout { get; init; } = 10000;
    public int PingInterval { get; init; } = 10000;
}