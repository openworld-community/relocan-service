using Autofac;
using ReloCAN.Service.Core.Interfaces;
using ReloCAN.Service.Core.Services;

namespace ReloCAN.Service.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();

    builder.RegisterType<DeleteUserService>()
        .As<IDeleteUserService>().InstancePerLifetimeScope();
  }
}

