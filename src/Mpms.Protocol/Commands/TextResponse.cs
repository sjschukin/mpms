using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

public class TextResponse : ResponseBase
{
    public string? Text { get; private set; }
    
    public override void ParseData(byte[] data)
    {
        Text = Parser.GetString(data);
    }
}