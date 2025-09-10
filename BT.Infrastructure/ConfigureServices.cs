using System.Text.Json.Serialization;
using BT.Domain.Models.Settings;
using BT.Infrastructure.Configurations;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories;
using BT.Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BT.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BeautyTradingContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"),
                builder => builder.MigrationsAssembly(typeof(BeautyTradingContext).Assembly.FullName));
        });
        services.AddScoped<IUnitOfWork<BeautyTradingContext>, UnitOfWork<BeautyTradingContext>>();
        services.AddScoped<BeautyTradingContextSeed>();
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddJwt(configuration);
        services.AddSwagger();
        services.AddCors();
        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            // x.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        });
        return services;
    }
}