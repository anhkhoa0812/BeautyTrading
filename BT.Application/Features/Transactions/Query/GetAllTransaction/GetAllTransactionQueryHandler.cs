using BT.Application.Common.Exceptions;
using BT.Application.Services.Implement;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.Transactions;
using BT.Infrastructure.Paginate.Interface;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.Transactions.Query.GetAllTransaction;

public class GetAllTransactionQueryHandler : IRequestHandler<GetAllTransactionQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public GetAllTransactionQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    
    public async ValueTask<ApiResponse> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
    {
        Guid accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId));
        if (account == null)
        {
            throw new NotFoundException("Account not found");
        }

        IPaginate<GetTransactionResponse> transactions;

        if (account.Role.Equals(ERole.Admin))
        {
            transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => new GetTransactionResponse
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Status = t.Status,
                    TransactionReference = t.TransactionReference,
                },
                predicate: t => t.Status.Equals(ETransactionStatus.Completed),
                page: request.Page,
                size: request.Size);
        }

        else
        {
            transactions = await _unitOfWork.GetRepository<Transaction>().GetPagingListAsync(
                selector: t => new GetTransactionResponse
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Status = t.Status,
                    TransactionReference = t.TransactionReference,
                },
                predicate: t => t.Order.AccountId.Equals(accountId),
                include: t => t.Include(t => t.Order),
                page: request.Page,
                size: request.Size);
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Transactions retrieved",
            Data = transactions
        };
    }
}