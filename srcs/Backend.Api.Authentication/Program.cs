using Backend.Libs.Domain.Extensions;
using Backend.Libs.Infrastructure.Enums;
using Backend.Libs.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddRabbitMqProducer(builder.Configuration);

builder.Services.AddGrpcDatabaseServices();
builder.Services.AddDomainLibs(typeof(Program).Namespace!);
builder.Services.AddInfrastructureLibs(builder.Configuration);

var app = builder.Build();

app.UseDomainLibs();

app.Urls.Add($"http://*:{(short)ServicePort.AUTHENTICATION_API_PORT}");

app.UsePathBase("/api");

await app.RunAsync();
await app.WaitForShutdownAsync();