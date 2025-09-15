namespace BT.Application.Services.Interface;

public interface IPayPalService
{
    public Task<string> CreateUrlPayment(decimal amount, string description);
}