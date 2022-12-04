using Mpms.Common;
using Mpms.Protocol;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<MpdConnectionOptions>(options =>
    builder.Configuration.Bind(MpdConnectionOptions.SECTION_NAME, options));

// Add services to the container
MpdConnectionOptions? mpdConnectionOptions = builder.Configuration.Get<MpdConnectionOptions>();

if (mpdConnectionOptions is null)
    throw new Exception("MPD connection section is not found in configuration file.");

switch (mpdConnectionOptions.Type)
{
    case "UnixSocket":
        builder.Services.AddSingleton<IMpdClient, UnixSocketClient>();
        break;
    case "Network":
        builder.Services.AddSingleton<IMpdClient, NetworkClient>();
        break;
    default:
        throw new Exception("MPD connection type is not recognized. Supported: UnixSocket, Network.");
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();