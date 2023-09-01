using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using ReloCAN.Service.Core;
using ReloCAN.Service.Infrastructure;
using ReloCAN.Service.Worker.HostedServices;
using ReloCAN.Service.Worker.Messaging;
using ReloCAN.Service.Worker.Messaging.Consumers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext(connectionString!);

builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // all services in container for diagnostics
  //TODO remove
  config.Path = "/serviceslist";
});

builder.Services.AddHostedService<MigrationService>();

builder.Services.AddMassTransit(x =>
{
  x.UsingRabbitMq((context, cfg) =>
  {
    var settings = builder.Configuration.GetRequiredSection("RabbitMQ").Get<RabbitMqSettings>();
    var host = settings?.Host ?? throw new ArgumentNullException(nameof(settings.Host), "RabbitMQ settings missing");
    var user = settings.Username ?? throw new ArgumentNullException(nameof(settings.Username), "RabbitMQ settings missing");
    var password = settings.Password ?? throw new ArgumentNullException(nameof(settings.Password), "RabbitMQ settings missing");
    cfg.AutoStart = true;
    cfg.Host(host,
      virtualHost: "/",
      h =>
      {
        h.Username(user);
        h.Password(password);
      });
    
    cfg.ReceiveEndpoint("test-message-consumer", e =>
    {
      e.Consumer<TestMessageConsumer>(context);
    });
  });
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
  containerBuilder.RegisterModule(new DefaultMessagingModule());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware();
}
else
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}
app.UseRouting();

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
