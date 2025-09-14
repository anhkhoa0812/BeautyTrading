using Mediator;
using BT.Application.Common.Behaviours;
using BT.Application.Common.Utils;
using BT.Application.Features.Accounts.Command.CreateAccount;
using BT.Application.Features.Authentication.Command.Login;
using BT.Application.Features.Orders.Command.CreateOrder;
using BT.Application.Services.Implement;
using BT.Application.Services.Interface;
using FluentValidation;

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
        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountCommandValidator>();
        services.AddScoped<IValidator<CreateOrderCommand>, CreateOrderCommandValidator>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPayPalService, PayPalService>();
        services.AddHttpContextAccessor();
        return services;
    }
}