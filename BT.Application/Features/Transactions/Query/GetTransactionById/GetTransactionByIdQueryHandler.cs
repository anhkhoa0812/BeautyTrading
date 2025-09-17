using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.Transactions;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.Transactions.Query.GetTransactionById;

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    
    public GetTransactionByIdQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    public async ValueTask<ApiResponse> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        Guid accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Account not found");
        
        GetTransactionResponse transaction;
        if (account.Role.Equals(ERole.Admin))
        {
            transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                selector: t => new GetTransactionResponse
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Status = t.Status,
                    TransactionReference = t.TransactionReference,
                },
                predicate: t => t.Id.Equals(request.Id) && t.Status.Equals(ETransactionStatus.Completed)) ?? throw new NotFoundException("Transaction not found");;
        }
        else
        {
            transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                selector: t => new GetTransactionResponse
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    Status = t.Status,
                    TransactionReference = t.TransactionReference,
                },
                predicate: t => t.Id.Equals(request.Id) && t.Order.AccountId.Equals(accountId),
                include: t => t.Include(t => t.Order));
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Transaction found",
            Data = transaction,
        };
    }
}