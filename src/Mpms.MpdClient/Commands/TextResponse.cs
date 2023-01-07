using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

public class TextResponse : ResponseBase
{
    public string? Text { get; private set; }

    public override void ParseData(byte[] data)
    {
        Text = GetString(data);
    }
}