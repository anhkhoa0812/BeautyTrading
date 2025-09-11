using System.Net;
using BT.Application.Common.Exceptions;
using BT.Application.Common.Utils;
using BT.Domain.Models.Common;

namespace BT.Application.Common.Middlewares;

public class GlobalException
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public GlobalException(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex)
        {
            await HandleBadRequestException(context, ex);
        }
        catch (NotFoundException ex)
        {
            await HandleNotFoundException(context, ex);
        }
        catch (ValidationException ex)
        {
            await HandleValidationException(context, ex);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }
    private static Task HandleNotFoundException(HttpContext context, NotFoundException ex)
    {
        int statusCode = (int)HttpStatusCode.NotFound;
        var errorResponse = new ApiResponse()
        {
            Status = (int) HttpStatusCode.NotFound,
            Message = "Data not found error",
            Data = ex.Message
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsync(JsonUtil.JsonString(errorResponse));
    }
    private static Task HandleValidationException(HttpContext context, ValidationException ex)
    {
        var errors = ex.Errors.Select(error => new
            {
                PropertyName = error.Key, 
                ErrorMessage = error.Value
            })
            .DistinctBy(error => error.PropertyName)
            .ToArray();
    
        int statusCode = (int)HttpStatusCode.BadRequest;
        var errorResponse = new ApiResponse()
        {
            Status = (int) HttpStatusCode.BadRequest,
            Message = "Data validation error",
            Data = errors
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        return context.Response.WriteAsync(JsonUtil.JsonString(errorResponse));
    }
    
    private static Task HandleBadRequestException(HttpContext context, BadHttpRequestException ex)
    {
        var statusCode = HttpStatusCode.BadRequest;
        var errorResponse = new ApiResponse()
        {
            Status = (int) statusCode,
            Message = "Data validation error",
            Data = ex.Message,
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        
        return context.Response.WriteAsync(JsonUtil.JsonString(errorResponse));
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorResponse = new ApiResponse()
        {
            Status = (int) statusCode,
            Message = "An unexpected error has occurred",
            Data = ex.Message,
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        
        return context.Response.WriteAsync(JsonUtil.JsonString(errorResponse));
    }
}