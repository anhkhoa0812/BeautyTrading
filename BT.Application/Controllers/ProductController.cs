using BT.Application.Common.Utils;
using BT.Application.Common.Validators;
using BT.Application.Features.Products.CreateProduct;
using BT.Domain.Constants;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Product.ProductEndpoint)]
public class ProductController : BaseController<ProductController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<CreateProductCommand> _createProductValidationUtil;
    
    public ProductController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateProductCommand> createProductValidationUtil) : base(logger)
    {
        _mediator = mediator;
        _createProductValidationUtil = createProductValidationUtil;
    }
    
    [CustomAuthorize(ERole.Admin)]
    [HttpPost(ApiEndPointConstant.Product.ProductEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
    {
        var (isValid, response) = await _createProductValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return StatusCode(400, response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateProduct), apiResponse);
    }
    
}