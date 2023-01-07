using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

/// <summary>
/// Shows information about all outputs.
/// </summary>
public class OutputsRequest : RequestBase
{
    public OutputsRequest() : base("outputs\n")
    {
    }
}