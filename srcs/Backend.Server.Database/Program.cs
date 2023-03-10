using Backend.Libs.Domain.Extensions;
using Backend.Libs.Infrastructure.Enums;
using Backend.Libs.Infrastructure.Extensions;
using Backend.Libs.Infrastructure.Services.Account;
using Backend.Libs.Persistence.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x => x.ConfigureEndpointDefaults(co => co.Protocols = HttpProtocols.Http2));

builder.Services.AddGrpcDatabaseServices();
builder.Services.AddPersistenceLibs();
builder.Services.AddDomainLibs(typeof(Program).Namespace!);
builder.Services.AddInfrastructureLibs(builder.Configuration);
builder.Services.TryAddRabbitMqProducer(builder.Configuration);
builder.Services.AddCodeFirstGrpc(config =>
{
    config.MaxReceiveMessageSize = null;
    config.EnableDetailedErrors = true;
});

var app = builder.Build();

app.UseDomainLibs();

app.Urls.Add($"http://*:{(short)ServicePort.DATABASE_SERVER_PORT}");

app.MapGrpcService<GrpcAccountService>();

await app.TryMigrateEfCoreDatabaseAsync();
await app.RunAsync();
await app.WaitForShutdownAsync();