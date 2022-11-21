using Mpms.Common;

namespace Mpms.Protocol.Base;

public abstract class ResponseBase : IMpdResponse
{
    protected readonly ResponseParser Parser;

    protected ResponseBase()
    {
        Parser = new ResponseParser();
    }

    public virtual void ParseData(byte[] data)
    {
    }
}