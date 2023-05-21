using Autofac;
using ReloCAN.Service.Worker.Messaging.Consumers;

namespace ReloCAN.Service.Worker.Messaging;

public class DefaultMessagingModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<TestMessageConsumer>()
      .AsSelf()
      .InstancePerLifetimeScope();
  }
}

