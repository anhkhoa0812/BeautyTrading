using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace BT.Application.Features.ProductColors.Command.CreateProductColor;

public class CreateProductColorCommandHandler : IRequestHandler<CreateProductColorCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IUploadService _uploadService;
    
    public CreateProductColorCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    
    public async ValueTask<ApiResponse> Handle(CreateProductColorCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.GetRepository<Product>().SingleOrDefaultAsync(
            predicate: x => request.ProductId == x.Id,
            include: x => x.Include(x => x.ProductColors)
        );
        if (product == null)
        {
            throw new NotFoundException("Product not found");
        }
        var existingColor = product.ProductColors?.FirstOrDefault(x => x.ColorName.ToLower() == request.ColorName.ToLower());
        if (existingColor != null)
        {
            throw new BadHttpRequestException("Color already exists for this product");
        }

        var productColor = new ProductColor()
        {
            Id = Guid.CreateVersion7(),
            ColorName = request.ColorName,
            ProductId = product.Id
        };
        var imageUrl = await _uploadService.UploadImageAsync(request.Image);
        productColor.ImageUrl = imageUrl;
        await _unitOfWork.GetRepository<ProductColor>().InsertAsync(productColor);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            _logger.Error("Failed to create product color for product {ProductId}", product.Id);
            throw new Exception("Failed to create product color");
        }
        _logger.Information("Product color {ColorName} created successfully for product {ProductId}", productColor.ColorName, product.Id);
        return new ApiResponse
        {
            Status = StatusCodes.Status201Created,
            Message = "Product color created successfully",
            Data = product.Id
        };
    }
}