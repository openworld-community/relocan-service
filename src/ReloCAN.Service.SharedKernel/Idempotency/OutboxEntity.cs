namespace ReloCAN.Service.SharedKernel.Idempotency;

public class OutboxEntity
{
  public string? IdempotencyId { get; set; }
  public string? Response { get; set; }
  public string? Events { get; set; }
  public string? Commands { get; set; }
  public bool IsDispatched { get; set; }
}
