using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

/// <summary>
/// Shows information about all outputs.
/// </summary>
public class OutputsRequest : RequestBase
{
    public override byte[] GetData()
    {
        string command = "outputs\n";
        
        return Parser.GetBytes(command);
    }
}