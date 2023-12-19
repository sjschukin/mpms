using System.Text;
using System.Text.RegularExpressions;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

public partial class PingResponse(byte[] inputData) : ResponseBase(inputData)
{
    public bool IsOk { get; } = ResponseRegex().IsMatch(Encoding.UTF8.GetString(inputData));

    [GeneratedRegex("^OK$", RegexOptions.IgnoreCase)]
    private static partial Regex ResponseRegex();
}