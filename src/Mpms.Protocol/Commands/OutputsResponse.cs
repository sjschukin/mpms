using System.Text.RegularExpressions;
using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

/// <summary>
/// Shows information about all outputs.
/// </summary>
public class OutputsResponse : ResponseBase
{
    private const string PATTERN = @"^(?<key>\S+):\s(?<value>.+)$";
    
    /// <summary>
    /// ID of the output.
    /// </summary>
    public int OutputId { get; private set; }
    
    /// <summary>
    /// Name of the output. It can be any.
    /// </summary>
    public string? OutputName { get; private set; }
    
    /// <summary>
    /// Plugin name currently in use.
    /// </summary>
    public string? Plugin { get; private set; }
    
    /// <summary>
    /// Status of the output. False if disabled, True if enabled.
    /// </summary>
    public bool OutputEnabled { get; private set; }
    
    /// <summary>
    /// Attribute.
    /// </summary>
    public string? Attribute { get; private set; }

    public override void ParseData(byte[] data)
    {
        string inputString = Parser.GetString(data);
        
        foreach(Match match in Regex.Matches(inputString, PATTERN, RegexOptions.Multiline))
        {
            string value = match.Groups["value"].Value;
            
            switch (match.Groups["key"].Value)
            {
                case "outputid":
                    OutputId = Int32.Parse(value);
                    break;
                case "outputname":
                    OutputName = value;
                    break;
                case "plugin":
                    Plugin = value;
                    break;
                case "outputenabled":
                    OutputEnabled = Boolean.Parse(value);
                    break;
                case "attribute":
                    Attribute = value;
                    break;
            }
        }
    }
}