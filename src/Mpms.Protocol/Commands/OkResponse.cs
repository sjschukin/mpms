using System.Text.RegularExpressions;
using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

public class OkResponse : ResponseBase
{
    private const string PATTERN = @"^OK$";
    
    public bool? IsOk { get; private set; }
    
    public override void ParseData(byte[] data)
    {
        string inputString = Parser.GetString(data);
        IsOk = Regex.IsMatch(inputString, PATTERN, RegexOptions.IgnoreCase);
    }
}