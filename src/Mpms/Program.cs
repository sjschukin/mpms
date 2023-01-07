using Mpms.MpdClient;
using Mpms.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddMpdClient(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddHostedService<ClientService>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();