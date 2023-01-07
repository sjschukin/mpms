using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

public class ByteResponse : ResponseBase
{
    public byte[]? Data { get; private set; }

    public override void ParseData(byte[] data)
    {
        Data = data;
    }
}