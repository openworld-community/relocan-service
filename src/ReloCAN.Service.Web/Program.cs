using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReloCAN.Service.Core;
using ReloCAN.Service.Infrastructure;
using ReloCAN.Service.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using ReloCAN.Service.SharedKernel.Idempotency;
using ReloCAN.Service.SharedKernel.Messages;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext(connectionString!);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReloCAN API", Version = "v1" });
  c.EnableAnnotations();
});

builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // all services in container for diagnostics
  //TODO remove
  config.Path = "/serviceslist";
});

builder.Services.AddMassTransit(x =>
{
  x.UsingRabbitMq((_, cfg) =>
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
  });
});

builder.Services.AddIdempotency<UnitOfWork>(c =>
{
  c.DispatchWithMassTransit();
  c.PersistWithEfCore(s =>
  {
    var optionsBuilder = s.GetRequiredService<DbContextOptionsBuilder<AppDbContext>>();

    return new AppDbContext(optionsBuilder.Options, null);
  }, o =>
  {
    o.OutboxDeserializer.AddAssembly(typeof(TestMessage).Assembly);
  });
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new DefaultInfrastructureModule(builder.Environment.EnvironmentName == "Development"));
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

app.UseStaticFiles();
app.UseCookiePolicy();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReloCAN API V1"));

app.MapDefaultControllerRoute();

// Seed Database
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;

  var context = services.GetRequiredService<AppDbContext>();
  if (context.Database.IsRelational()) // WebApplicationFactory for functional tests uses InMemoryDb
  {
    var migrations = context.Database.GetPendingMigrations();
    if (migrations.Any())
    {
      // Pending migration will be applied in MigrationService within Worker component
      throw new InvalidOperationException("There are pending migrations, aborting.");
    }
  }
}

app.Run();

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
