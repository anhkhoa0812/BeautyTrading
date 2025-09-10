using BT.Domain.Entities;
using BT.Domain.Entities.Common.Interface;
using BT.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace BT.Infrastructure.Persistence;

public class BeautyTradingContext : DbContext
{
    public BeautyTradingContext() {}
    
    public BeautyTradingContext(DbContextOptions<BeautyTradingContext> options) : base(options) { }
    
    public virtual DbSet<Account> Account { get; set; } = null!;
    public virtual DbSet<Category> Category { get; set; } = null!;
    public virtual DbSet<Order> Order { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItem { get; set; } = null!;
    public virtual DbSet<PaymentMethod> PaymentMethod { get; set; } = null!;
    public virtual DbSet<Product> Product { get; set; } = null!;
    public virtual DbSet<ProductImage> ProductImage { get; set; } = null!;
    public virtual DbSet<ProductVariant> ProductVariant { get; set; } = null!;
    public virtual DbSet<Transaction> Transaction { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BeautyTradingContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                            e.State == EntityState.Added ||
                            e.State == EntityState.Deleted);

            foreach (var item in modified)
                switch (item.State)
                {
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addedEntity)
                        {
                            addedEntity.CreatedDate = TimeUtil.GetCurrentSEATime();
                            item.State = EntityState.Added;
                        }

                        break;
                    case EntityState.Modified:
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            Entry(item.Entity).Property("Id").IsModified = false;
                            modifiedEntity.LastModifiedDate = TimeUtil.GetCurrentSEATime();
                            item.State = EntityState.Modified;
                        }

                        break;
                }

            var result = await base.SaveChangesAsync(cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
            
            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}