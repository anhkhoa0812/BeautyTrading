using BT.Application.Common.Exceptions;
using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.ProductVariants.Command.UpdateProductVariant;

public class UpdateProductVariantCommandHandler : IRequestHandler<UpdateProductVariantCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    
    public UpdateProductVariantCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async ValueTask<ApiResponse> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var productVariant = await _unitOfWork.GetRepository<ProductVariant>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.ProductVariantId,
            include: x => x.Include(x => x.Product)
        );

        if (productVariant == null)
        {
            throw new NotFoundException("Product variant not found");
        }

        productVariant.Name = request.Name ?? productVariant.Name;
        if (request.Name != null && !productVariant.Product.IsHasVariants)
        {
            productVariant.Product.Name = request.Name;
        }
        productVariant.Price = request.Price ?? productVariant.Price;
        productVariant.Currency = request.Currency ?? productVariant.Currency;
        productVariant.Stock = request.Stock ?? productVariant.Stock;
        productVariant.IsActive = request.IsActive ?? productVariant.IsActive;
        
        _unitOfWork.GetRepository<ProductVariant>().UpdateAsync(productVariant);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Failed to update product variant");
        }
        
        _logger.Information("Product variant with id {ProductVariantId} updated successfully", productVariant.Id);

        return new ApiResponse()
        {
            Status = StatusCodes.Status200OK,
            Message = "Product variant updated successfully",
            Data = productVariant.Id
        };
    }
}