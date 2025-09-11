using BT.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[Route(ApiEndPointConstant.ApiEndpoint)]
[ApiController]
public class BaseController<T> : ControllerBase where T : BaseController<T>
{
    protected ILogger _logger;

    public BaseController(ILogger logger)
    {
        _logger = logger;
    }
}