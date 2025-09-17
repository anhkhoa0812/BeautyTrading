using BT.Application.Features.Transactions.Query.GetAllTransaction;
using BT.Application.Features.Transactions.Query.GetTransactionById;
using BT.Domain.Constants;
using BT.Domain.Models.Common;
using BT.Domain.Models.Transactions;
using BT.Infrastructure.Paginate.Interface;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

public class TransactionController : BaseController<TransactionController>
{
    private readonly IMediator _mediator;

    public TransactionController(ILogger logger, IMediator mediator) : base(logger)
    {
        _mediator = mediator;
    }
    
    [HttpGet(ApiEndPointConstant.Transaction.GetAllTransaction)]
    [ProducesResponseType<ApiResponse<IPaginate<GetTransactionResponse>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllTransaction([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var query = new GetAllTransactionQuery()
        {
            Page = page,
            Size = size
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    } 
    
    [HttpGet(ApiEndPointConstant.Transaction.GetTransactionById)]
    [ProducesResponseType<ApiResponse<GetTransactionResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactionById([FromRoute] Guid id)
    {
        var query = new GetTransactionByIdQuery()
        {
            Id = id
        };

        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    } 
}