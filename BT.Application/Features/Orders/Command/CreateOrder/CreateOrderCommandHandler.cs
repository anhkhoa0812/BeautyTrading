using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.OrderItems;
using BT.Domain.Models.Orders;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using BT.Infrastructure.Utils;
using Mediator;

namespace BT.Application.Features.Orders.Command.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public CreateOrderCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger,
        IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    
    public async ValueTask<ApiResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Guid accountId = _claimService.GetCurrentUserId;

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Account not found");

        var order = new Order
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            Status = EOrderStatus.Pending,
            CreatedDate = TimeUtil.GetCurrentSEATime(),
            TotalPrice = 0,
            LastModifiedDate = TimeUtil.GetCurrentSEATime(),
        };
        
        foreach (var item in request.Items)
        {
            var productVariant = await _unitOfWork.GetRepository<ProductVariant>().SingleOrDefaultAsync(
                predicate: p => p.Id.Equals(item.ProductVariantId)) ?? throw new NotFoundException("Product not found");

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductVariantId = item.ProductVariantId,
                Price = productVariant.Price,
                Quantity = item.Quantity
            };
            order.OrderItems.Add(orderItem);
            order.TotalPrice += orderItem.Price * orderItem.Quantity;
        }
        
        await _unitOfWork.GetRepository<Order>().InsertAsync(order);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Creating order failed");
        }
        
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Order created",
            Data = new CreateOrderResponse()
            {
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                OrderItems = order.OrderItems.Select(oi => new CreateOrderItemsResponse()
                {
                    Id = oi.Id,
                    ProductVariantId = oi.ProductVariantId,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            }
        };
    }
}