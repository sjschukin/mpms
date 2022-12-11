using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

/// <summary>
/// Turns an output off.
/// </summary>
public class DisableOutputRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="id">ID of the output</param>
    public DisableOutputRequest(int id)
    {
        Id = id;
    }

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int Id { get; }

    public override byte[] GetData()
    {
        string command = $"disableoutput {Id}\n";
        
        return Parser.GetBytes(command);
    }
}