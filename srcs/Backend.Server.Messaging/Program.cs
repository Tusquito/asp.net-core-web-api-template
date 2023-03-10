using Backend.Libs.Application.Extensions;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Infrastructure.Enums;
using Backend.Libs.Infrastructure.Extensions;
using Backend.Libs.Infrastructure.Messages;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(x => x.ConfigureEndpointDefaults(co => co.Protocols = HttpProtocols.Http2));

builder.Services.AddDomainLibs(typeof(Program).Namespace!);
builder.Services.AddInfrastructureLibs(builder.Configuration);
builder.Services.AddOptionsValidatorsLibsOnly();

builder.Services.TryAddRabbitMqConsumer<TestMessage>(builder.Configuration);

var app = builder.Build();

app.UseDomainLibs();

app.Urls.Add($"http://*:{(short)ServicePort.MESSAGING_SERVER_PORT}");

await app.RunAsync();
await app.WaitForShutdownAsync();