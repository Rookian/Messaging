using Core;
using Wolverine;
using Wolverine.SqlServer;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.UseWolverine(opts =>
{
    opts.UseSqlServerPersistenceAndTransport(connectionString, "sender",

            // If using Sql Server as a queue between multiple applications,
            // be sure to use the same transportSchema setting
            transportSchema: "transport")
        .AutoProvision()
        .AutoPurgeOnStartup();

    // opts.PublishMessage<SqlServerFoo>().ToSqlServerQueue("foobar");
    // opts.PublishMessage<SqlServerBar>().ToSqlServerQueue("foobar");
    // opts.Policies.DisableConventionalLocalRouting();
    // opts.Discovery.DisableConventionalDiscovery();
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// builder.Host.UseWolverine(opts =>
// {
//     opts.PersistMessagesWithSqlServer(connectionString, "wolverine");
//     opts.UseEntityFrameworkCoreTransactions();
//     opts.Policies.UseDurableLocalQueues();
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/send", async (IMessageBus bus) => { await bus.SendAsync(new SqlServerFoo()); })
    .WithName("SendMessage")
    .WithOpenApi();

app.Run();