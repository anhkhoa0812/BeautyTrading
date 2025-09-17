namespace BT.Domain.Models.Products;

public class GetProductByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsHasVariants { get; set; }
    public string? VideoUrl { get; set; }
    public string BannerUrl { get; set; }
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
    public List<ProductVariantForGetProductByIdResponse> ProductVariants { get; set; }
    public List<ProductImageForGetProductByIdResponse>? ProductImages { get; set; }
    public List<ProductColorForGetProductByIdResponse>? ProductColors { get; set; }
}
public class ProductVariantForGetProductByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public int Stock { get; set; }
    public bool IsActive { get; set; }
}
public class ProductImageForGetProductByIdResponse
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; }
}

public class ProductColorForGetProductByIdResponse
{
    public Guid Id { get; set; }
    public string ColorName { get; set; }
    public string ImageUrl { get; set; }
}