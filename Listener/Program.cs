using Core;
using Wolverine;
using Wolverine.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.UseWolverine(opts =>
{
    opts.UseSqlServerPersistenceAndTransport(connectionString,
            "listener",

            transportSchema: "transport")
        .AutoProvision()
        .AutoPurgeOnStartup();
    opts.PublishMessage<SqlServerBar>().ToSqlServerQueue("foobar");
    opts.ListenToSqlServerQueue("foobar");
    opts.Discovery.DisableConventionalDiscovery()
        .IncludeType<FooBarHandler>();
});
// Add services to the container.
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

app.MapGet("/weatherforecast", () =>
    {

    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

public class FooBarHandler
{
    public void Handle(Core.SqlServerFoo foo)
    {
        
    }
}

