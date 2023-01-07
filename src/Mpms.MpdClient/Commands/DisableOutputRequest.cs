using Mpms.MpdClient.Base.Commands;

namespace Mpms.MpdClient.Commands;

/// <summary>
/// Turns an output off.
/// </summary>
public class DisableOutputRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="id">ID of the output</param>
    public DisableOutputRequest(int id) : base($"disableoutput {id}\n")
    {
        Id = id;
    }

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int Id { get; }
}