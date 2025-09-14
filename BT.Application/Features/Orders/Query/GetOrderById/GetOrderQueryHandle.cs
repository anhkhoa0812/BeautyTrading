using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.OrderItems;
using BT.Domain.Models.Orders;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.Orders.Query.GetOrderById;

public class GetOrderQueryHandle : IRequestHandler<GetOrderQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public GetOrderQueryHandle(IUnitOfWork<BeautyTradingContext> unitOfWork, 
        ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    public async ValueTask<ApiResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        Guid accountId = _claimService.GetCurrentUserId;

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Account not found");

        GetOrderDetailResponse order;
        
        if (account.Role.Equals(ERole.Admin))
        {
            order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                selector: o => new GetOrderDetailResponse()
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                    Items = o.OrderItems.Select(oi => new GetOrderItemsResponse()
                    {
                        Id = oi.Id,
                        Name = oi.ProductVariant.Name,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        ProductVariantId = oi.ProductVariantId
                    }).ToList()
                },
                predicate: o => o.Id.Equals(request.Id),
                include: o => o.Include(o => o.OrderItems)
                                                .ThenInclude(oi => oi.ProductVariant)
                                                .ThenInclude(p => p.Product));
        }

        else
        {
            order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(
                selector: o => new GetOrderDetailResponse()
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                    Items = o.OrderItems.Select(oi => new GetOrderItemsResponse()
                    {
                        Id = oi.Id,
                        Name = oi.ProductVariant.Name,
                        Price = oi.Price,
                        Quantity = oi.Quantity,
                        ProductVariantId = oi.ProductVariantId
                    }).ToList()
                },
                predicate: o => o.Id.Equals(request.Id) && o.AccountId.Equals(accountId),
                include: o => o.Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                    .ThenInclude(p => p.Product));
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Get order",
            Data = order
        };
    }
}