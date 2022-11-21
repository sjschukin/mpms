using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

/// <summary>
/// Reports the current status of the player and the volume level.
/// </summary>
public class StatusRequest : RequestBase
{
    public override byte[] GetData()
    {
        string command = "status\n";
        
        return Parser.GetBytes(command);
    }
}