using System.Text.Json.Serialization;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Wakett;
using Wakett.Application.Commands;
using Wakett.Application.Services;
using Wakett.BackgroundServices;
using Wakett.Common.Contracts;
using Wakett.Common.DTOs;
using Wakett.Common.Implementations;
using Wakett.Domain.Events;
using Wakett.Domain.Handlers;
using Wakett.Infrastructure.Clients;
using Wakett.Infrastructure.Persistence;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RateService>();
builder.Services.AddScoped<PositionService>();
builder.Services.AddScoped<CoinMarketCapClient>();
builder.Services.AddScoped<PositionAddedHandler>();
builder.Services.AddScoped<PositionClosedEventHandler>();
builder.Services.AddHostedService<PositionEventProcessor>();
builder.Services.AddSingleton(sp => 
    new ServiceBusClient(builder.Configuration["AzureServiceBus:ConnectionString"]));
builder.Services.AddHostedService<RateSchedulerSimulator>();

builder.Services.AddSingleton<IMessageBus, AzureServiceBusMessageBus>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddDbContext<RatesDbContext>(options =>
    options.UseInMemoryDatabase("RatesDb"));

builder.Services.AddDbContext<PositionsDbContext>(options =>
    options.UseInMemoryDatabase("PositionsDb"));

builder.Services.Configure<RouteOptions>(options =>
{
    options.SetParameterPolicy<RegexInlineRouteConstraint>("regex");
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.MapPost("/api/positions", async (AddPositionCommand command, [FromServices] IMediator mediator) =>
    {
        await mediator.Publish(new PositionAddedEvent(
            command.InstrumentId,
            command.Quantity,
            command.InitialRate,
            command.Side));

        return Results.Accepted();
    })
    .WithName("AddPosition");

app.MapDelete("/api/positions/{id}", async (Guid id, [FromServices] IMediator mediator) =>
    {
        await mediator.Publish(new PositionClosedEvent(id));
        return Results.NoContent();
    })
    .WithName("ClosePosition");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PositionsDbContext>();
    await DatabaseSeeder.SeedAsync(dbContext);
}
app.Run();

namespace Wakett
{
    [JsonSerializable(typeof(AddPositionCommand[]))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
    [JsonSerializable(typeof(CryptoDataResponse))]
    public partial class MyJsonContext : JsonSerializerContext
    {
    }
}