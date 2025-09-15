using BT.Application.Common.Utils;
using BT.Application.Common.Validators;
using BT.Application.Features.Categories.Command.CreateCategory;
using BT.Application.Features.Categories.Command.UpdateCategory;
using BT.Application.Features.Categories.Query.GetCategories;
using BT.Application.Features.Categories.Query.GetCategoryById;
using BT.Domain.Constants;
using BT.Domain.Enums;
using BT.Domain.Models.Categories;
using BT.Domain.Models.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Category.CategoryEndpoint)]
public class CategoryController : BaseController<CategoryController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<CreateCategoryCommand> _createCategoryValidationUtil;
    private readonly ValidationUtil<UpdateCategoryCommand> _updateCategoryValidationUtil;
    public CategoryController(IMediator mediator, ILogger logger,
        ValidationUtil<CreateCategoryCommand> createCategoryValidationUtil,
        ValidationUtil<UpdateCategoryCommand> updateCategoryValidationUtil) : base(logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _createCategoryValidationUtil = createCategoryValidationUtil ?? throw new ArgumentNullException(nameof(createCategoryValidationUtil));
        _updateCategoryValidationUtil = updateCategoryValidationUtil ?? throw new ArgumentNullException(nameof(updateCategoryValidationUtil));
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPost(ApiEndPointConstant.Category.CategoryEndpoint)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var (isValid, response) = await _createCategoryValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateCategory), apiResponse);
    }
    [HttpGet(ApiEndPointConstant.Category.CategoryEndpoint)]
    [ProducesResponseType<ApiResponse<GetCategoriesResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategories([FromQuery] int page = 1, [FromQuery] int size = 30,
        [FromQuery] string? sortBy = null, [FromQuery] bool isAsc = false)
    {
        var query = new GetCategoriesQuery()
        {
            Page = page,
            Size = size,
            SortBy = sortBy,
            IsAsc = isAsc
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [HttpGet(ApiEndPointConstant.Category.CategoryWithId)]
    [ProducesResponseType<ApiResponse<GetCategoryByIdResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
        var query = new GetCategoryByIdQuery()
        {
            CateogoryId = id
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }

    [CustomAuthorize(ERole.Admin)]
    [HttpPatch(ApiEndPointConstant.Category.CategoryWithId)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand()
        {
            CategoryId = id,
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };
        var (isValid, response) = await _updateCategoryValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}