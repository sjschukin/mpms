using System.Text.RegularExpressions;
using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

/// <summary>
/// Shows updating status.
/// </summary>
public class UpdateResponse : ResponseBase
{
    private const string PATTERN = @"^(?<key>\S+):\s(?<value>.+)$";

    /// <summary>
    /// Positive number identifying the update job.
    /// </summary>
    public int UpdatingDatabaseJobId { get; private set; }

    public override void ParseData(byte[] data)
    {
        string inputString = Parser.GetString(data);

        foreach(Match match in Regex.Matches(inputString, PATTERN, RegexOptions.Multiline))
        {
            string value = match.Groups["value"].Value;

            switch (match.Groups["key"].Value)
            {
                case "updating_db":
                    UpdatingDatabaseJobId = Int32.Parse(value);
                    break;
            }
        }
    }
}