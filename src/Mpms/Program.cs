using Mpms.Mpd.Client;
using Mpms.Mpd.Client.Extensions;
using Mpms.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services
    .Configure<MpdConnectionOptions>(builder.Configuration.GetSection(MpdConnectionOptions.SECTION_NAME));

// Add services to the container
builder.Services
    .AddMpdClient()
    .AddHostedService<ClientBackgroundService>()
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app
    .UseHttpsRedirection()
    .UseAuthorization();

app.MapControllers();

await app.RunAsync();