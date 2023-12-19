using System.Text;
using System.Text.RegularExpressions;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Shows information about all outputs.
/// </summary>
public partial class OutputsResponse : ResponseBase
{
    public OutputsResponse(byte[] inputData) : base(inputData)
    {
        string inputString = Encoding.UTF8.GetString(inputData);

        foreach(Match match in ResponseRegex().Matches(inputString))
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

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int OutputId { get; }

    /// <summary>
    /// Name of the output. It can be any.
    /// </summary>
    public string? OutputName { get; }

    /// <summary>
    /// Plugin name currently in use.
    /// </summary>
    public string? Plugin { get; }

    /// <summary>
    /// Status of the output. False if disabled, True if enabled.
    /// </summary>
    public bool OutputEnabled { get; }

    /// <summary>
    /// Attribute.
    /// </summary>
    public string? Attribute { get; }

    [GeneratedRegex(@"^(?<key>\S+):\s(?<value>.+)$", RegexOptions.Multiline)]
    private static partial Regex ResponseRegex();
}