namespace Mpms.Common;

public interface IMpdClient : IDisposable
{
    string? ProtocolVersion { get; set; }
    void Run();
    void Stop();
    TResponse SendRequest<TResponse>(IMpdRequest request) where TResponse : IMpdResponse, new();
}