using Serilog;
using BT.Application.Common.Extensions;
using BT.Application.Common.Middlewares;
using BT.Infrastructure;
using BT.Infrastructure.Configurations;
using BT.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(SerilogConfig.Configure);
Log.Information("Starting Beauty Trading API up");

try
{
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddHttpClient();
    var app = builder.Build();

    app.UseDefaultFiles();
    app.UseStaticFiles();
    if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Beauty Trading API V1");
            c.InjectStylesheet("/assets/css/kkk.css");
        });
    }

    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var beautyTradingContextSeed = scope.ServiceProvider.GetRequiredService<BeautyTradingContextSeed>();
            await beautyTradingContextSeed.InitializeAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "An error occurred while seeding the database.");
            throw;
        }
    }

    app.UseCors(builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    app.UseMiddleware<GlobalException>();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled: {ex.Message}");
}
finally
{
    Log.Information("Shut down Beauty Trading API complete");
    Log.CloseAndFlush();
}