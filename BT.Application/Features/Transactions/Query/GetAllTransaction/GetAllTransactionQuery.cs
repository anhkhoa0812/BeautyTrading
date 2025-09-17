using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Transactions.Query.GetAllTransaction;

public class GetAllTransactionQuery : IRequest<ApiResponse>
{
    public int Page { get; set; }
    public int Size { get; set; }
}