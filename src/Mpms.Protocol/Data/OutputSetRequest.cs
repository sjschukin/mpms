using Mpms.Protocol.Base;

namespace Mpms.Protocol.Data;

/// <summary>
/// Set a runtime attribute. These are specific to the output plugin, and supported values are
/// usually printed in the <see cref="OutputsResponse"/>.
/// </summary>
public class OutputSetRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="id">ID of the output</param>
    /// <param name="name">Attribute name</param>
    /// <param name="value">Attribute value</param>
    public OutputSetRequest(int id, string name, string value)
    {
        Id = id;
        Name = name;
        Value = value;
    }

    /// <summary>
    /// ID of the output.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Attribute name.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Attribute value.
    /// </summary>
    public string Value { get; }

    public override byte[] GetData()
    {
        string command = $"outputset {Id} {Name} {Value}\n";
        
        return Parser.GetBytes(command);
    }
}