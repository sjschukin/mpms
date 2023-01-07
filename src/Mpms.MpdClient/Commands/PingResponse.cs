using System.Text.RegularExpressions;
using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

public class PingResponse : ResponseBase
{
    private const string PATTERN = @"^OK$";

    public bool? IsOk { get; private set; }

    public override void ParseData(byte[] data)
    {
        string inputString = GetString(data);
        IsOk = Regex.IsMatch(inputString, PATTERN, RegexOptions.IgnoreCase);
    }
}