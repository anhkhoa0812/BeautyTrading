using Mediator;
using BT.Application.Common.Behaviours;
using BT.Application.Common.Utils;
using BT.Application.Services.Implement;
using BT.Application.Services.Interface;

namespace BT.Application.Common.Extensions;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddMediator( options =>
            {
                options.Namespace = "BT.Application.Controllers";
                options.ServiceLifetime = ServiceLifetime.Scoped;
            })
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped(typeof(ValidationUtil<>));
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}