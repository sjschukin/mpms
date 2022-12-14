using System.Net.Sockets;

namespace Mpms.MpdClient.Extensions;

internal static class ExtensionMethods
{
    private const int BUFFER_SIZE = 1024;

    public static byte[] ReadAllData(this NetworkStream stream)
    {
        byte[] allData = Array.Empty<byte>();

        do
        {
            var data = new byte[BUFFER_SIZE];
            int bytes = stream.Read(data, 0, data.Length);

            byte[] allDataTemp = allData;
            allData = new byte[allDataTemp.Length + bytes];

            if (allDataTemp.Length > 0)
                Buffer.BlockCopy(allDataTemp, 0, allData, 0, allDataTemp.Length);
            
            Buffer.BlockCopy(data, 0, allData, allDataTemp.Length, bytes);

        } while (stream.DataAvailable);

        return allData;
    }
}