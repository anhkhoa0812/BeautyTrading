using System.Linq.Expressions;
using BT.Domain.Entities;

namespace BT.Infrastructure.Filter.Models;

public class ProductFilter : IFilter<Product>
{
    public string? Name { get; set; }
    public Guid? CategoryId { get; set; }
    public Expression<Func<Product, bool>> ToExpression()
    {
        return member =>
            (string.IsNullOrEmpty(Name) || member.Name.Contains(Name)) &&
            (CategoryId != null || member.CategoryId == CategoryId);
    }
}