using BT.Application.Common.Utils;
using BT.Application.Common.Validators;
using BT.Application.Features.ProductColors.Command.CreateProductColor;
using BT.Application.Features.Products.Query.GetProductById;
using BT.Application.Features.Products.Query.GetProducts;
using BT.Application.Features.Products.Command.CreateProduct;
using BT.Domain.Constants;
using BT.Domain.Enums;
using BT.Domain.Models.Common;
using BT.Domain.Models.Products;
using BT.Infrastructure.Paginate.Interface;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Product.ProductEndpoint)]
public class ProductController : BaseController<ProductController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<CreateProductCommand> _createProductValidationUtil;
    private readonly ValidationUtil<CreateProductColorCommand> _createProductColorValidationUtil;
    public ProductController(ILogger logger, IMediator mediator,
        ValidationUtil<CreateProductCommand> createProductValidationUtil,
        ValidationUtil<CreateProductColorCommand> createProductColorValidationUtil) : base(logger)
    {
        _mediator = mediator;
        _createProductValidationUtil = createProductValidationUtil;
        _createProductColorValidationUtil = createProductColorValidationUtil;
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
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateProduct), apiResponse);
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPost(ApiEndPointConstant.Product.ProductWithProductColor)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProductColor([FromRoute] Guid id, [FromForm] CreateProductColorRequest request)
    {
        var command = new CreateProductColorCommand
        {
            ProductId = id,
            ColorName = request.ColorName,
            Image = request.Image
        };
        var (isValid, response) = await _createProductColorValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateProductColor), apiResponse);
    }

    [HttpGet(ApiEndPointConstant.Product.ProductEndpoint)]
    [ProducesResponseType<ApiResponse<IPaginate<GetProductsResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
    {
        var query = new GetProductsQuery()
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    [HttpGet(ApiEndPointConstant.Product.ProductWithId)]
    [ProducesResponseType<ApiResponse<GetProductByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        var query = new GetProductByIdQuery()
        {
            ProductId = id
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
}