using System.Text;
using Mpms.Common;

namespace Mpms.Protocol;

public class ResponseParser
{
    public TResponse Parse<TResponse>(byte[] data)
        where TResponse : IMpdResponse, new()
    {
        TResponse response = new TResponse();
        
        response.ParseData(data);

        return response;
    }

    public string GetString(byte[] data)
    {
        return Encoding.UTF8.GetString(data);
    }

    public byte[] GetBytes(string data)
    {
        return Encoding.UTF8.GetBytes(data);
    }
}