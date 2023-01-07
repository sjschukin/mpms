using System.Text;
using Mpms.Common;

namespace Mpms.MpdClient.Base.Commands;

public abstract class RequestBase : IRequest
{
    protected RequestBase(string commandText)
    {
        Data = Encoding.UTF8.GetBytes(commandText);
    }

    public byte[] Data { get; }
}