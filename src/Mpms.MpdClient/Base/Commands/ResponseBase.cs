using System.Text;
using Mpms.Common;

namespace Mpms.MpdClient.Base.Commands;

public abstract class ResponseBase : IResponse
{
    public virtual void ParseData(byte[] data) => throw new NotImplementedException();

    protected string GetString(byte[] data) => Encoding.UTF8.GetString(data);
}