using Backend.Libs.Application.Extensions;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Infrastructure.Enums;
using Backend.Libs.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddRabbitMqProducer(builder.Configuration);
        
builder.Services.AddGrpcDatabaseServices();
builder.Services.AddDomainLibs(typeof(Program).Namespace!);
builder.Services.AddInfrastructureLibs(builder.Configuration);
builder.Services.AddApplicationLibs();

var app = builder.Build();

app.UseDomainLibs();

app.Urls.Add($"http://*:{(short)ServicePort.TEST_API_PORT}");

app.UsePathBase("/api/tests/");

await app.RunAsync();
await app.WaitForShutdownAsync();