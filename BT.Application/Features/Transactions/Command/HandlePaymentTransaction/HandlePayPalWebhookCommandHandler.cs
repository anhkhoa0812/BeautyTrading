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
    private readonly IClaimService _claimService;
    private readonly IPayPalService _payPalService;

    public HandlePayPalWebhookCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger,
        IClaimService claimService, IPayPalService payPalService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
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
        
        if (eventType == "PAYMENT.CAPTURE.COMPLETED")
        {
            var captureId = resource.GetProperty("id").GetString();
            var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(
                predicate: t => t.TransactionReference.Equals(captureId));

            if (transaction == null)
            {
                _logger.Error($"Transaction not found for PayPal captureId {captureId}");
                return Unit.Value;
            }

            transaction.Status = ETransactionStatus.Completed;
            _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
            _logger.Information($"Transaction {transaction.Id} updated to status {transaction.Status}");
            await _unitOfWork.CommitAsync();
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