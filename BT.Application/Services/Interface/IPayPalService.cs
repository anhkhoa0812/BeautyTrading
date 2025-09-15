using BT.Domain.Entities;
using BT.Domain.Models.Payment;

namespace BT.Application.Services.Interface;

public interface IPayPalService
{
    public Task<PayPalCreateOrder> CreateUrlPayment(Order order, string currency, string description);
}