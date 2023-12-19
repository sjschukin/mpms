using System.Text;
using Mpms.Common;

namespace Mpms.Mpd.Common;

public abstract class ResponseBase(byte[] inputData) : IResponse
{
    public byte[] Data { get; } = inputData;
}