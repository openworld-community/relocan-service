using System.Diagnostics;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using ReloCAN.Service.Core.UserAggregate;
using ReloCAN.Service.Infrastructure.Data;
using ReloCAN.Service.SharedKernel.Idempotency;
using ReloCAN.Service.SharedKernel.Messages;

namespace ReloCAN.Service.Web.Api;

public class MetaController : BaseApiController
{
  private readonly IUnitOfWorkManager<UnitOfWork> _uowManager;

  public MetaController(IUnitOfWorkManager<UnitOfWork> uowManager)
  {
    _uowManager = uowManager;
  }

  [HttpGet("/info")]
  public async Task<ActionResult<string>> Info()
  {
    await using var uow = await _uowManager.Begin("SomethingUniqueFromTheRequest");

    if (!uow.Outbox.IsClosed)
    {
      var newUser = new User(DateTime.UtcNow.ToLongDateString());
      await uow.Users.AddAsync(newUser);
      uow.Outbox.Publish(new TestMessage {Data = $"User created! {newUser.Name}"});

      await uow.Commit();
    }
    await uow.EnsureOutboxDispatched();
    
    var assembly = typeof(Program).Assembly;

    var creationDate = System.IO.File.GetCreationTime(assembly.Location);
    var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

    return Ok($"Version: {version}, Last Updated: {creationDate}");
  }
}
