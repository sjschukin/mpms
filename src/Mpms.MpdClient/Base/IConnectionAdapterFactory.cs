using Mpms.Common;

namespace Mpms.MpdClient.Base;

public interface IConnectionAdapterFactory
{
    IConnectionAdapter GetConnectionAdapter(string type);
}