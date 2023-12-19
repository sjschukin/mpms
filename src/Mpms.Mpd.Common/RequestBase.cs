using System.Text;
using Mpms.Common;

namespace Mpms.Mpd.Common;

public abstract class RequestBase(string commandText) : IRequest
{
    public byte[] Data { get; } = Encoding.UTF8.GetBytes(commandText);
}