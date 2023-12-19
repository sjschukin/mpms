using Mpms.Mpd.Common;

namespace Mpms.Mpd.Client.Commands;

public class ByteResponse(byte[] inputData) : ResponseBase(inputData);