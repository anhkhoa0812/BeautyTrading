using BT.Application.Common.Utils;
using BT.Application.Common.Validators;
using BT.Application.Features.ProductVariants.Command.UpdateProductVariant;
using BT.Domain.Constants;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.ProductVariant.ProductVariantEndpoint)]
public class ProductVariantController : BaseController<ProductVariantController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<UpdateProductVariantCommand> _updateProductVariantValidationUtil;
    
    
    public ProductVariantController(ILogger logger, IMediator mediator,
        ValidationUtil<UpdateProductVariantCommand> updateProductVariantValidationUtil) : base(logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _updateProductVariantValidationUtil = updateProductVariantValidationUtil ?? throw new ArgumentNullException(nameof(updateProductVariantValidationUtil));
    }
    
    [CustomAuthorize(ERole.Admin)]
    [HttpPatch(ApiEndPointConstant.ProductVariant.ProductVariantWithId)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProductVariant([FromRoute] Guid id, [FromBody] UpdateProductVariantRequest request)
    {
        var command = new UpdateProductVariantCommand()
        {
            ProductVariantId = id,
            Name = request.Name,
            Price = request.Price,
            Currency = request.Currency,
            Stock = request.Stock,
            IsActive = request.IsActive
        };
        
        var (isValid, response) = await _updateProductVariantValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}