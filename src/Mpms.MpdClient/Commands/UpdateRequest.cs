using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

/// <summary>
/// Updates the music database: find new files, remove deleted files, update modified files.
/// </summary>
public class UpdateRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="uri">A particular directory or song/file to update. If you do not specify it, everything is updated.</param>
    public UpdateRequest(string? uri = null) : base(String.IsNullOrWhiteSpace(uri) ? "update\n" : $"update {uri}\n")
    {
        Uri = uri;
    }

    /// <summary>
    /// A particular directory or song/file to update.
    /// </summary>
    public string? Uri { get; }
}