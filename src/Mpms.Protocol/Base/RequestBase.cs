using Mpms.Common;

namespace Mpms.Protocol.Base;

public abstract class RequestBase : IMpdRequest
{
    protected readonly ResponseParser Parser;

    protected RequestBase()
    {
        Parser = new ResponseParser();
    }

    public virtual byte[] GetData()
    {
        throw new NotImplementedException();
    }
}