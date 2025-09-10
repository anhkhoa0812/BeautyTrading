using Microsoft.EntityFrameworkCore;

namespace BT.Infrastructure.Persistence;

public class BeautyTradingContextSeed
{
    private readonly ILogger _logger;
    private readonly BeautyTradingContext _context;
    
    public BeautyTradingContextSeed(ILogger logger, BeautyTradingContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task InitializeAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.Error(e, "An error occurred while migrating the database");
            throw;
        }
    }
}