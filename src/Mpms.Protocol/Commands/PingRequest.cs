using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

/// <summary>
/// Does nothing but return “OK”.
/// </summary>
public class PingRequest : RequestBase
{
    public override byte[] GetData()
    {
        string command = "ping\n";
        
        return Parser.GetBytes(command);
    }
}