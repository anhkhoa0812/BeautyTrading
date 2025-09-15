using Mediator;
using BT.Application.Common.Behaviours;
using BT.Application.Common.Utils;
using BT.Application.Features.Accounts.Command.CreateAccount;
using BT.Application.Features.Authentication.Command.Login;
using BT.Application.Features.Orders.Command.CreateOrder;
using BT.Application.Features.Categories.Command.CreateCategory;
using BT.Application.Features.Categories.Command.UpdateCategory;
using BT.Application.Features.ProductColors.Command.CreateProductColor;
using BT.Application.Features.Products.Command.CreateProduct;
using BT.Application.Features.ProductVariants.Command.CreateProductVariant;
using BT.Application.Features.ProductVariants.Command.UpdateProductVariant;
using BT.Application.Services.Implement;
using BT.Application.Services.Interface;
using BT.Domain.Models.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
        services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
        services.AddScoped<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>();
        services.AddScoped<IValidator<CreateProductColorCommand>, CreateProductColorCommandValidator>();
        services.AddScoped<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();
        services.AddScoped<IValidator<CreateProductVariantCommand>, CreateProductVariantCommandValidator>();
        services.AddScoped<IValidator<UpdateProductVariantCommand>, UpdateProductVariantCommandValidator>();
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IPayPalService, PayPalService>();
        services.AddScoped<IUploadService, UploadService>();
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                var apiResponse = new ApiResponse()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "Data validation error",
                    Data = errors
                };

                return new BadRequestObjectResult(apiResponse);
            };
        });
        services.AddHttpContextAccessor();
        return services;
    }
}