using Mpms.Common;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Microsoft.Extensions.Options;
using Mpms.Protocol.Data;
using Timer = System.Timers.Timer;

namespace Mpms.Protocol;

public class NetworkClient : IMpdClient
{
    private readonly TcpClient _client;
    private NetworkStream? _stream;
    private readonly ResponseParser _parser;
    private readonly Timer _heartBeatTimer;
    private bool _isBusy;

    public NetworkClient(IOptions<MpdConnectionOptions> configuration)
    {
        _client = new TcpClient();
        _parser = new ResponseParser();
        Parameters = configuration.Value;
        _heartBeatTimer = new Timer(Parameters.HeartBeatInterval) {AutoReset = true};

        _heartBeatTimer.Elapsed += OnHeartBeatTimerElapsed;
    }

    public MpdConnectionOptions Parameters { get; }
    public string? ProtocolVersion { get; set; }

    public void Run()
    {
        EstablishConnection();
        _heartBeatTimer.Start();
    }

    public void Stop()
    {
        _heartBeatTimer.Stop();
    }

    private void EstablishConnection()
    {
        IPAddress? address = Dns.GetHostEntry(Parameters.Address).AddressList.FirstOrDefault();

        if (address is null)
            throw new Exception("Cannot resolve host address.");

        var endPoint = new IPEndPoint(address, Parameters.Port);

        _client.SendTimeout = Parameters.CommandTimeout;
        _client.ReceiveTimeout = Parameters.CommandTimeout;
        _client.Connect(endPoint);
        _stream = _client.GetStream();

        VersionResponse response = _parser.Parse<VersionResponse>(_stream.ReadAllData());
        ProtocolVersion = response.Version;
    }

    public TResponse SendRequest<TResponse>(IMpdRequest request) where TResponse : IMpdResponse, new()
    {
        if (_stream is null)
            throw new Exception("Stream cannot be null.");

        var sendBuffer = request.GetData();

        try
        {
            _isBusy = true;
            _stream.Write(sendBuffer, 0, sendBuffer.Length);

            return _parser.Parse<TResponse>(_stream.ReadAllData());
        }
        finally
        {
            _isBusy = false;
        }
    }

    private void OnHeartBeatTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_isBusy)
            return;

        SendRequest<OkResponse>(new PingRequest());
    }

    public void Dispose()
    {
        _heartBeatTimer.Dispose();
        _stream?.Dispose();
        _client.Dispose();
    }
}