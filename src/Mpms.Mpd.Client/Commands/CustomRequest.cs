using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Command with a custom command text.
/// </summary>
public class CustomRequest : RequestBase
{
    /// <summary>
    /// Creates an instance of the command.
    /// </summary>
    /// <param name="text">Command text</param>
    public CustomRequest(string text) : base($"{text}\n")
    {
        Text = text;
    }

    /// <summary>
    /// Command text.
    /// </summary>
    public string Text { get; }
}