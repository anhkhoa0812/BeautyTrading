namespace BT.Domain.Models.Vat;

public class VatCheckResponse
{
    public bool Valid { get; set; }
    public string VatNumber { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string CountryCode { get; set; }
}