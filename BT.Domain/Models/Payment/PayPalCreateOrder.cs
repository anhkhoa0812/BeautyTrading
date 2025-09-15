namespace BT.Domain.Models.Payment;

public class PayPalCreateOrder
{
    public string Code { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}