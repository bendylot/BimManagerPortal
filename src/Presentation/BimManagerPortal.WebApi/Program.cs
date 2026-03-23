using BimManagerPortal.Application;
using BimManagerPortal.Persistance;
using BimManagerPortal.WebApi;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, _, config) =>
{
    var seqUrl = ctx.Configuration["Seq:Url"] ?? "http://localhost:5341";

    config
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
        .WriteTo.File(
            "logs/mybackend-.log",
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 7)
        .WriteTo.Seq(seqUrl);
});

builder.Services.AddApplication();
builder.Services.AddPresentation();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.ConfigurePresentation();

app.Run();
