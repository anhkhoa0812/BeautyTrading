using BT.Application.Common.Exceptions;
using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.ProductVariants.Command.CreateProductVariant;

public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public CreateProductVariantCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async ValueTask<ApiResponse> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductId && x.IsHasVariants
        );

        if (product == null)
        {
            throw new NotFoundException("Product not found or product does not support variants");
        }

        var productVariant = new ProductVariant()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Price = request.Price,
            Currency = request.Currency,
            Stock = request.Stock,
            IsActive = true,
            ProductId = request.ProductId
        };
        
        await _unitOfWork.GetRepository<ProductVariant>().InsertAsync(productVariant);
        
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Failed to create product variant for product {ProductId}", request.ProductId);
            throw new Exception("Failed to create product variant");
        }
        
        return new ApiResponse
        {
            Status = StatusCodes.Status201Created,
            Message = "Product variant created successfully",
            Data = product.Id
        };
    }
}