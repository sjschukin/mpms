using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

public class TextResponse : ResponseBase
{
    public string? Text { get; private set; }
    
    public override void ParseData(byte[] data)
    {
        Text = Parser.GetString(data);
    }
}