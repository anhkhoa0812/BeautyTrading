using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.Orders;
using BT.Infrastructure.Paginate;
using BT.Infrastructure.Paginate.Interface;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Orders.Query.GetAllOrder;

public class GetAllOrderQueryHandle : IRequestHandler<GetAllOrderQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;

    public GetAllOrderQueryHandle(IUnitOfWork<BeautyTradingContext> unitOfWork, 
        ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    
    public async ValueTask<ApiResponse> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {
        Guid accountId = _claimService.GetCurrentUserId;

        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: a => a.Id.Equals(accountId)) ?? throw new NotFoundException("Account not found");

        IPaginate<GetOrderResponse> orders;
        
        if (account.Role.Equals(ERole.Admin))
        {
            orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: o => new GetOrderResponse()
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                },
                predicate: o => !o.Status.Equals(EOrderStatus.Pending),
                page: request.Page,
                size: request.Size);
        }

        else
        {
            orders = await _unitOfWork.GetRepository<Order>().GetPagingListAsync(
                selector: o => new GetOrderResponse()
                {
                    Id = o.Id,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice,
                },
                predicate: o => o.AccountId.Equals(accountId),
                page: request.Page,
                size: request.Size);
        }

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Get Orders successfully",
            Data = orders
        };
    }
}