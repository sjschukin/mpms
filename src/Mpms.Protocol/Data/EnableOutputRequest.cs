using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

/// <summary>
/// Turns an output on.
/// </summary>
public class EnableOutputRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="id">ID of the output</param>
    public EnableOutputRequest(int id)
    {
        Id = id;
    }

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int Id { get; }

    public override byte[] GetData()
    {
        string command = $"enableoutput {Id}\n";
        
        return Parser.GetBytes(command);
    }
}