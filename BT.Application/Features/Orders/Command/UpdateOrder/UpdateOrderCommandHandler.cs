using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.Orders;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Orders.Command.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;

    public UpdateOrderCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork,
        ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
            predicate: o => o.Id.Equals(request.Id)) ?? throw new NullReferenceException("Order not found");

        if (!order.Status.Equals(EOrderStatus.Processing))
        {
            throw new Exception($"Order is not processing");
        }
        
        order.Status = request.Status;
        
        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;

        if (!isSuccess)
        {
            throw new Exception("Failed to update order");
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Order successfully updated",
            Data = new GetOrderResponse()
            {
                Id = order.Id,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
            }
        };
    }
}