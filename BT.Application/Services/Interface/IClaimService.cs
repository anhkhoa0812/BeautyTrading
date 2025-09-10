namespace BT.Application.Services.Interface;

public interface IClaimService
{
    public Guid GetCurrentUserId { get; }
    public string GetCurrentUsername { get; }
    public string GetRole { get; }
}