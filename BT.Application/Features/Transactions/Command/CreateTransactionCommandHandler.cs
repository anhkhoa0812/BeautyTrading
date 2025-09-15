using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.Transactions.Command;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    private readonly IPayPalService _payPalService;

    public CreateTransactionCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService,
        IPayPalService payPalService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
        _payPalService = payPalService;
    }
    public async ValueTask<ApiResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        Guid accountId = _claimService.GetCurrentUserId;
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Account not found");

        var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
            predicate: o => o.Id.Equals(request.OrderId),
            include: o => o.Include(o => o.OrderItems)
                                                .ThenInclude(oi => oi.ProductVariant)
                                            .Include(o => o.OrderItems)
                                                .ThenInclude(oi => oi.ProductColor)) ?? throw new NotFoundException("Order not found");
        
        if (order.AccountId != accountId)
        {
            throw new BadHttpRequestException("Order is not authorized to perform this operation");
        }

        var paymentMethod = await _unitOfWork.GetRepository<PaymentMethod>().SingleOrDefaultAsync(
            predicate: p => p.Id.Equals(request.PaymentMethodId)) ?? throw new NotFoundException("Payment method not found");
        
        var url = await _payPalService.CreateUrlPayment(order, request.Currency, $"Payment for order {order.Id}");
        
        var transaction = new Transaction()
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Amount = order.TotalPrice,
            Status = ETransactionStatus.Pending,
            Currency = request.Currency,
            TransactionReference = url.Code,
            PaymentMethodId = request.PaymentMethodId,
        };
        
        await _unitOfWork.GetRepository<Transaction>().InsertAsync(transaction);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Failed to create transaction");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Transaction created successfully",
            Data = url.Url
        };
    }
}