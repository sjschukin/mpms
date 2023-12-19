using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

/// <summary>
/// Shows information about all outputs.
/// </summary>
public class OutputsRequest() : RequestBase("outputs\n");