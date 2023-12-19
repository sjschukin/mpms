using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Turns an output on.
/// </summary>
public class EnableOutputRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="id">ID of the output</param>
    public EnableOutputRequest(int id) : base($"enableoutput {id}\n")
    {
        Id = id;
    }

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int Id { get; }
}