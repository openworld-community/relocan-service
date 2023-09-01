namespace ReloCAN.Service.SharedKernel.Idempotency;

internal class ObjectEnvelope
{
  public string? Type { get; set; }
  public string? Body { get; set; }
}
