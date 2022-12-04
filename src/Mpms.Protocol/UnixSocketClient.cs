using System.Net.Sockets;
using System.Timers;
using Microsoft.Extensions.Options;
using Mpms.Common;
using Mpms.Protocol.Data;
using Timer = System.Timers.Timer;

namespace Mpms.Protocol;

public class UnixSocketClient : IMpdClient
{
    private readonly Socket _socket;
    private NetworkStream? _stream;
    private readonly ResponseParser _parser;
    private readonly Timer _heartBeatTimer;
    private bool _isBusy;

    public UnixSocketClient(IOptions<MpdConnectionOptions> configuration)
    {
        _socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
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
        var endPoint = new UnixDomainSocketEndPoint(Parameters.Address);

        _socket.SendTimeout = Parameters.CommandTimeout;
        _socket.ReceiveTimeout = Parameters.CommandTimeout;
        _socket.Connect(endPoint);
        _stream = new NetworkStream(_socket);

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

        var response = SendRequest<OkResponse>(new PingRequest());
    }

    public void Dispose()
    {
        _heartBeatTimer.Dispose();
        _stream?.Dispose();
        _socket.Dispose();
    }
}