using System.Text.Json;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Transactions.Command.HandlePaymentTransaction;

public class HandlePayPalWebhookCommandHandler : IRequestHandler<HandlePayPalWebhookCommand, Unit>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IPayPalService _payPalService;

    public HandlePayPalWebhookCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IPayPalService payPalService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _payPalService = payPalService;
    }

    public async ValueTask<Unit> Handle(HandlePayPalWebhookCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"Request body: {request.Body}, Headers: {request.Headers}");
        var isValid = await _payPalService.VerifyWebhookAsync(request.Body, request.Headers);
        if (!isValid)
        {
            _logger.Error("Webhook is not valid");
            return Unit.Value;
        }
        using var doc = JsonDocument.Parse(request.Body);
        var eventType = doc.RootElement.GetProperty("event_type").GetString();
        _logger.Information($"PayPal webhook received: {eventType}");
        var resource = doc.RootElement.GetProperty("resource");
        _logger.Information($"PayPal webhook resource: {resource}");
        
        if (eventType == "CHECKOUT.ORDER.APPROVED")
        {
            var orderId = resource.GetProperty("id").GetString();
            _logger.Information($"Order {orderId} approved");

            var captureId = await _payPalService.CaptureOrderAsync(orderId);
            _logger.Information($"CaptureId {captureId} for order {orderId} created");
            var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                predicate: t => t.TransactionReference.Equals(orderId));

            if (transaction == null)
            {
                _logger.Error($"Transaction not found for PayPal orderId {orderId}");
                return Unit.Value;
            }

            transaction.Status = ETransactionStatus.Completed;
            _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
            var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                predicate: o => o.Id.Equals(transaction.OrderId));
            order.Status = EOrderStatus.Processing;
            _unitOfWork.GetRepository<Order>().UpdateAsync(order);
            foreach (var orderItem in order.OrderItems)
            {
                var productVariant = await _unitOfWork.GetRepository<ProductVariant>().SingleOrDefaultAsync(
                    predicate: p => p.Id.Equals(orderItem.ProductVariantId));
                productVariant.Stock -= orderItem.Quantity;
                _unitOfWork.GetRepository<ProductVariant>().UpdateAsync(productVariant);
            }
            await _unitOfWork.CommitAsync();

            _logger.Information($"Transaction {transaction.Id} captured successfully with captureId {captureId}");
        }
        else if (eventType == "PAYMENT.CAPTURE.DENIED")
        {
            var captureId = resource.GetProperty("id").GetString();

            var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                predicate: t => t.TransactionReference.Equals(captureId));

            if (transaction == null)
            {
                _logger.Error($"Transaction not found for PayPal captureId {captureId}");
                return Unit.Value;
            }

            transaction.Status = ETransactionStatus.Rejected;
            _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
            _logger.Information($"Transaction {transaction.Id} updated to status {transaction.Status}");
            await _unitOfWork.CommitAsync();
        }
        return Unit.Value;
    }
}