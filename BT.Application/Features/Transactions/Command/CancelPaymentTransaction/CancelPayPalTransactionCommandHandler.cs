using BT.Application.Common.Exceptions;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Transactions.Command.CancelPaymentTransaction;

public class CancelPayPalTransactionCommandHandler : IRequestHandler<CancelPayPalTransactionCommand, bool>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;

    public CancelPayPalTransactionCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async ValueTask<bool> Handle(CancelPayPalTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
            predicate: t => t.TransactionReference.Equals(request.TransactionReference)) ?? throw new NotFoundException("Transaction not found");
        transaction.Status = ETransactionStatus.Rejected;
        _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);

        bool isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            return false;
        }

        return true;
    }
}