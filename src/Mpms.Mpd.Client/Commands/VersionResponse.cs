using System.Text;
using System.Text.RegularExpressions;
using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// When the client connects to the server, the server will answer with the following response.
/// </summary>
public partial class VersionResponse : ResponseBase
{
    public VersionResponse(byte[] inputData) : base(inputData)
    {
        string inputString = Encoding.UTF8.GetString(inputData);

        Match? match = ResponseRegex().Matches(inputString).FirstOrDefault();

        if (match is null)
            throw new Exception("Response is not recognized.");

        Name = match.Groups["name"].Value;
        Version = match.Groups["version"].Value;
    }

    /// <summary>
    /// Application name.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// A version identifier such as X.Y.Z. This version identifier is the version of the protocol spoken,
    /// not the real version of the daemon. (There is no way to retrieve this real version identifier from the connection.)
    /// </summary>
    public string? Version { get; }

    [GeneratedRegex(@"^OK\s(?<name>\S+)\s(?<version>\S+)$")]
    private static partial Regex ResponseRegex();
}