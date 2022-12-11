using Mpms.Protocol.Commands.Base;

namespace Mpms.Protocol.Commands;

/// <summary>
/// Command with a custom command text.
/// </summary>
public class CustomRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="text">Command text</param>
    public CustomRequest(string text)
    {
        Text = text;
    }

    /// <summary>
    /// Command text.
    /// </summary>
    public string Text { get; }

    public override byte[] GetData()
    {
        string command = $"{Text}\n";
        
        return Parser.GetBytes(command);
    }
}