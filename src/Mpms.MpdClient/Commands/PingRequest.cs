using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

/// <summary>
/// Does nothing but return “OK”.
/// </summary>
public class PingRequest : RequestBase
{
    public PingRequest() : base("ping\n")
    {
    }
}