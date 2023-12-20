using System.Net.Sockets;

namespace Mpms.Mpd.Client.Extensions;

internal static class NetworkStreamExtensions
{
    private const int BUFFER_SIZE = 1024;

    public static async Task<byte[]> ReadAllDataAsync(this NetworkStream stream, CancellationToken cancellationToken)
    {
        byte[] allData = Array.Empty<byte>();

        do
        {
            var data = new byte[BUFFER_SIZE];
            int bytes = await stream.ReadAsync(data, cancellationToken);

            byte[] allDataTemp = allData;
            allData = new byte[allDataTemp.Length + bytes];

            if (allDataTemp.Length > 0)
                Buffer.BlockCopy(allDataTemp, 0, allData, 0, allDataTemp.Length);

            Buffer.BlockCopy(data, 0, allData, allDataTemp.Length, bytes);

        } while (stream.DataAvailable);

        return allData;
    }
}