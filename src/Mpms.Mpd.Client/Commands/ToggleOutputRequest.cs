using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Turns an output on or off, depending on the current state.
/// </summary>
public class ToggleOutputRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="id">ID of the output</param>
    public ToggleOutputRequest(int id) : base($"disableoutput {id}\n")
    {
        Id = id;
    }

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int Id { get; }
}