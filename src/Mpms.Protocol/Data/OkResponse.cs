using System.Text.RegularExpressions;
using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

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