using BT.Domain.Entities.Common;

namespace BT.Domain.Entities;

public class Product : EntityAuditBase<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsHasVariants { get; set; }
    public string BannerUrl { get; set; }
    public string? VideoUrl { get; set; }
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
    public virtual Category Category { get; set; }
    public virtual ICollection<ProductImage>? ProductImages { get; set; } = new List<ProductImage>();
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<ProductColor>? ProductColors { get; set; } = new List<ProductColor>();
}