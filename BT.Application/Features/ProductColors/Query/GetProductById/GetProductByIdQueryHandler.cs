using BT.Application.Common.Exceptions;
using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Domain.Models.Products;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.ProductColors.Query.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public GetProductByIdQueryHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async ValueTask<ApiResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId,
            include: x => x.Include(x => x.ProductVariants)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
        );

        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }

        var response = new GetProductByIdResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            IsHasVariants = product.IsHasVariants,
            ProductVariants = product.ProductVariants.Select(pv => new ProductVariantForGetProductByIdResponse
            {
                Id = pv.Id,
                Name = pv.Name,
                Price = pv.Price,
                Currency = pv.Currency,
                Stock = pv.Stock,
                IsActive = pv.IsActive
            }).ToList(),
            ProductImages = product.ProductImages?.Select(pi => new ProductImageForGetProductByIdResponse
            {
                Id = pi.Id,
                ImageUrl = pi.ImageUrl
            }).ToList(),
            ProductColors = product.ProductColors?.Select(pc => new ProductColorForGetProductByIdResponse
            {
                Id = pc.Id,
                ColorName = pc.ColorName,
                ImageUrl = pc.ImageUrl
            }).ToList()
        };
        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Product retrieved successfully",
            Data = response
        };
    }
}