using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Transactions.Query.GetTransactionById;

public class GetTransactionByIdQuery : IRequest<ApiResponse>
{
    public Guid Id { get; set; }
}