using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Reports the current status of the player and the volume level.
/// </summary>
public class StatusRequest() : RequestBase("status\n");