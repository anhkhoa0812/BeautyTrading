using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Products.Command.CreateProduct;

public class CreateProductCommand : IRequest<ApiResponse>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile MainImage { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsHasVariants { get; set; }
    public IFormFile BannerImage { get; set; }
    public IFormFile? Video { get; set; }
    public string? Voltage { get; set; }
    public string? MachinePower { get; set; }
    public string? ControlMode { get; set; }
    public string? OutputFrequency { get; set; }
    public string? OutputPower { get; set; }
    public string? LedWavelength { get; set; }
    public string? LedOutputPower { get; set; }
    public string? IrFrequencyConversionInfraredLight { get; set; }	
    public string? IrInverterInfraredOutputPower { get; set; }
    public string? AirPumpNegativePressure { get; set; }
    public string? RotaryRfHandleTorqueMachineSetWeight { get; set; }
    public decimal MachineNetWeight { get; set; }
    public string? MachineSize { get; set; }
    public string? PackageSize { get; set; }
    public decimal PackageWeight { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
    public int? Stock { get; set; }
    public List<CreateProductVariantRequest>? Variants { get; set; }
    public List<CreateProductImageRequest>? Images { get; set; }
}

public class CreateProductVariantRequest
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
}
public class CreateProductImageRequest
{
    public IFormFile Image { get; set; }
}