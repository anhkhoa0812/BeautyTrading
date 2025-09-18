using BT.Domain.Models.Common;
using BT.Domain.Models.Vat;

namespace BT.Application.Services.Interface;

public interface IVatCheckService
{
    Task<VatCheckResponse> CheckVat(string vatNumber);
}