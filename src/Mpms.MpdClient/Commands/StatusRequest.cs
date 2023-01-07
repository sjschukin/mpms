using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

/// <summary>
/// Reports the current status of the player and the volume level.
/// </summary>
public class StatusRequest : RequestBase
{
    public StatusRequest() : base("status\n")
    {

    }
}