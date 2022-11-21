namespace Mpms.Protocol;

public class MpdUnixSocketParameters
{
    public string Path { get; init; } = "/var/run/mpd/socket";
    public int Timeout { get; init; } = 10000;
}