namespace Mpms.Protocol;

public class MpdConnectionOptions
{
    public const string SECTION_NAME = "MpdConnection";

    public string Type { get; set; } = "UnixSocket";
    public string Address { get; set; } = "/var/run/mpd/socket";
    public int Port { get; set; } = 6600;
    public int HeartBeatInterval { get; set; } = 10;
    public int CommandTimeout { get; set; } = 10;
}