using System.Text;
using System.Text.RegularExpressions;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Shows updating status.
/// </summary>
public partial class UpdateResponse : ResponseBase
{
    public UpdateResponse(byte[] inputData) : base(inputData)
    {
        string inputString = Encoding.UTF8.GetString(inputData);

        foreach(Match match in ResponseRegex().Matches(inputString))
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

    /// <summary>
    /// Positive number identifying the update job.
    /// </summary>
    public int UpdatingDatabaseJobId { get; }

    [GeneratedRegex(@"^(?<key>\S+):\s(?<value>.+)$", RegexOptions.Multiline)]
    private static partial Regex ResponseRegex();
}