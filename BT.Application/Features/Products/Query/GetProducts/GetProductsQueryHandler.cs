using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.Products;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.Products.Query.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IClaimService _claimService;
    public GetProductsQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IClaimService claimService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimService = claimService;
    }
    
    public async ValueTask<ApiResponse> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var role = _claimService.GetRole;
        var products = await _unitOfWork.GetRepository<Product>().GetPagingListAsync(
            selector: x => new GetProductsResponse()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                IsHasVariants = x.IsHasVariants,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate
            },
            predicate: x => (role == nameof(ERole.Admin) || x.ProductVariants.Any(x => x.IsActive))
                && (request.CategoryId == null || x.CategoryId == request.CategoryId)
                && (string.IsNullOrEmpty(request.Name) || x.Name.Contains(request.Name)),
            page: request.Page,
            size: request.Size,
            sortBy: request.SortBy ?? "CreatedDate",
            isAsc: request.IsAsc
        );
        
        return new ApiResponse
        {
            Status = StatusCodes.Status200OK,
            Message = "Get products successfully",
            Data = products
        };
    }
}