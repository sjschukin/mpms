using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Does nothing but returns “OK”.
/// </summary>
public class PingRequest() : RequestBase("ping\n");