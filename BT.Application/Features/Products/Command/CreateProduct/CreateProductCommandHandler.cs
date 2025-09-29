using BT.Application.Common.Exceptions;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IUploadService _uploadService;
    
    public CreateProductCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger, IUploadService uploadService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
    }
    public async ValueTask<ApiResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var existingCategory = await _unitOfWork.GetRepository<Category>().SingleOrDefaultAsync(
            predicate: x => x.Id == request.CategoryId
        );
        if (existingCategory == null)
        {
            throw new NotFoundException("Category not found");
        }
        var product = new Product()
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Description = request.Description,
            IsHasVariants = request.IsHasVariants,
            CategoryId = request.CategoryId,
            Voltage = request.Voltage,
            MachinePower = request.MachinePower,
            ControlMode = request.ControlMode,
            OutputFrequency = request.OutputFrequency,
            OutputPower = request.OutputPower,
            LedWavelength = request.LedWavelength,
            LedOutputPower = request.LedOutputPower,
            IrFrequencyConversionInfraredLight = request.IrFrequencyConversionInfraredLight,
            IrInverterInfraredOutputPower = request.IrInverterInfraredOutputPower,
            AirPumpNegativePressure = request.AirPumpNegativePressure,
            RotaryRfHandleTorqueMachineSetWeight = request.RotaryRfHandleTorqueMachineSetWeight,
            MachineNetWeight = request.MachineNetWeight,
            MachineSize = request.MachineSize,
            PackageSize = request.PackageSize,
            PackageWeight = request.PackageWeight,
        };
        if (request.Video != null)
        {
            var videoUrl = await _uploadService.UploadVideoAsync(request.Video);
            product.VideoUrl = videoUrl;
        }
        
        var mainImageTask = _uploadService.UploadImageAsync(request.MainImage);
        var bannerImageTask = _uploadService.UploadImageAsync(request.BannerImage);
        
        var imageResults = await Task.WhenAll(mainImageTask, bannerImageTask);
        product.ImageUrl = imageResults[0];
        product.BannerUrl = imageResults[1];
        if (!product.IsHasVariants)
        {
            if(request.Currency != null && request.Price != null && request.Stock != null)
            {
                var variant = new ProductVariant()
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = product.Id,
                    Name = request.Name,
                    Price = request.Price.Value,
                    Currency = request.Currency,
                    Stock = request.Stock.Value,
                    IsActive = true
                };
                product.ProductVariants.Add(variant);
            }
            else
            {
                throw new BadHttpRequestException("Currency, Price and Stock are required when product has no variants");
            }
        }
        else
        {
            if (request.Variants != null && request.Variants.Any())
            {
                foreach (var variantRequest in request.Variants)
                {
                    product.ProductVariants.Add(new ProductVariant()
                    {
                        Id = Guid.CreateVersion7(),
                        Name = variantRequest.Name,
                        Currency = variantRequest.Currency,
                        IsActive = true,
                        Price = variantRequest.Price,
                        ProductId = product.Id,
                        Stock = variantRequest.Stock
                    });
                }
            }
            else
            {
                throw new BadHttpRequestException("At least one variant is required when product has variants");
            }
        }

        if (request.Images != null && request.Images.Any())
        {
            foreach (var image in request.Images)
            {
                var imageUrl = await _uploadService.UploadImageAsync(image);
                product.ProductImages?.Add(new ProductImage()
                {
                    Id = Guid.CreateVersion7(),
                    ImageUrl = imageUrl,
                    ProductId = product.Id
                });
            }
        }
        await _unitOfWork.GetRepository<Product>().InsertAsync(product);

        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Create product failed");
        }
        
        _logger.Information("Product {ProductId} created successfully", product.Id);
        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Message = "Create product successfully",
            Data = product.Id
        };
        
    }
}