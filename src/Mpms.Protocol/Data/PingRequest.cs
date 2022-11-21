using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

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