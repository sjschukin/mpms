using System.Text;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

public class TextResponse(byte[] inputData) : ResponseBase(inputData)
{
    public string Text { get; } = Encoding.UTF8.GetString(inputData);
}