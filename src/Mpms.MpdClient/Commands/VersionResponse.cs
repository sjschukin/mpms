using System.Text.RegularExpressions;
using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

/// <summary>
/// When the client connects to the server, the server will answer with the following response.
/// </summary>
public class VersionResponse : ResponseBase
{
    private const string PATTERN = @"^OK\s(?<name>\S+)\s(?<version>\S+)$";

    /// <summary>
    /// Application name.
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// A version identifier such as X.Y.Z. This version identifier is the version of the protocol spoken,
    /// not the real version of the daemon. (There is no way to retrieve this real version identifier from the connection.)
    /// </summary>
    public string? Version { get; private set; }

    public override void ParseData(byte[] data)
    {
        string inputString = GetString(data);

        Match? match = Regex.Matches(inputString, PATTERN, RegexOptions.IgnoreCase).FirstOrDefault();

        if (match is null)
            throw new Exception("Response is not recognized.");

        Name = match.Groups["name"].Value;
        Version = match.Groups["version"].Value;
    }
}