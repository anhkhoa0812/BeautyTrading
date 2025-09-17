using BT.Application.Common.Utils;
using BT.Application.Features.Orders.Command.CreateOrder;
using BT.Application.Features.Orders.Command.UpdateOrder;
using BT.Application.Features.Orders.Query.GetAllOrder;
using BT.Application.Features.Orders.Query.GetOrderById;
using BT.Application.Features.Transactions.Command;
using BT.Application.Features.Transactions.Command.CreateTransaction;
using BT.Application.Services.Interface;
using BT.Domain.Constants;
using BT.Domain.Models.Common;
using BT.Domain.Models.Orders;
using BT.Infrastructure.Paginate.Interface;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

public class OrderController : BaseController<OrderController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<CreateOrderCommand> _createOrderCommand;
    private readonly ValidationUtil<CreateTransactionCommand> _createTransactionCommand;
    private readonly ValidationUtil<UpdateOrderCommand> _updateOrderCommand;

    public OrderController(ILogger logger, IMediator mediator, 
        ValidationUtil<CreateOrderCommand> createOrderCommand, ValidationUtil<CreateTransactionCommand> createTransactionCommand, ValidationUtil<UpdateOrderCommand> updateOrderCommand) : base(logger)
    {
        _mediator = mediator;
        _createOrderCommand = createOrderCommand;
        _createTransactionCommand = createTransactionCommand;
        _updateOrderCommand = updateOrderCommand;
    }
    
    [HttpPost(ApiEndPointConstant.Order.CreateOrder)]
    [ProducesResponseType<ApiResponse<CreateOrderResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var (isValid, response) = await _createOrderCommand.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    } 
    
    [HttpGet(ApiEndPointConstant.Order.GetAllOrder)]
    [ProducesResponseType<ApiResponse<IPaginate<GetOrderResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllOrder([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var query = new GetAllOrderQuery()
        {
            Page = page,
            Size = size
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    } 
    
    [HttpGet(ApiEndPointConstant.Order.GetOrder)]
    [ProducesResponseType<ApiResponse<IPaginate<GetOrderDetailResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder([FromRoute] Guid id)
    {
        var query = new GetOrderQuery()
        {
            Id = id
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }
    
    [HttpPut(ApiEndPointConstant.Order.UpdateOrder)]
    [ProducesResponseType<ApiResponse<GetOrderResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderRequest request)
    {
        var command = new UpdateOrderCommand()
        {
            Id = id,
            Status = request.Status,
        };

        var (isValid, response) = await _updateOrderCommand.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
    
    [HttpPost(ApiEndPointConstant.Order.PaymentOrder)]
    [ProducesResponseType<ApiResponse<string>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PaymentOrder([FromRoute] Guid id, [FromBody] CreateTransactionDTO request)
    {
        var command = new CreateTransactionCommand()
        {
            OrderId = id,
            Currency = request.Currency,
            PaymentMethodId = request.PaymentMethodId,
        };
        var (isValid, response) = await _createTransactionCommand.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    }
}