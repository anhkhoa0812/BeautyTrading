namespace BT.Domain.Models.Products;

public class GetProductByIdResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public bool IsHasVariants { get; set; }
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