using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

/// <summary>
/// Updates the music database: find new files, remove deleted files, update modified files.
/// </summary>
public class UpdateRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="uri">A particular directory or song/file to update. If you do not specify it, everything is updated.</param>
    public UpdateRequest(string? uri = null)
    {
        Uri = uri;
    }

    /// <summary>
    /// A particular directory or song/file to update.
    /// </summary>
    public string? Uri { get; }

    public override byte[] GetData()
    {
        string command = String.IsNullOrWhiteSpace(Uri) ? "update\n" : $"update {Uri}\n";

        return Parser.GetBytes(command);
    }
}