using System.Text;
using BT.Application.Features.Transactions.Command.CancelPaymentTransaction;
using BT.Application.Features.Transactions.Command.HandlePaymentTransaction;
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
    
    [HttpPost(ApiEndPointConstant.Transaction.Handle)]
    [ProducesResponseType<ApiResponse<Unit>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HandleTransaction()
    {
        Request.EnableBuffering();

        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
            Request.Body.Position = 0;
        }

        var query = new HandlePayPalWebhookCommand()
        {
            Body = body,
            Headers = Request.Headers
        };
        var apiResponse = await _mediator.Send(query);
        return Ok(apiResponse);
    }  
    
    [HttpPost(ApiEndPointConstant.Transaction.Cancel)]
    [ProducesResponseType<ApiResponse<Unit>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTransaction()
    {
        var orderId = Request.Query["orderId"].ToString();

        var query = new CancelPayPalTransactionCommand()
        {
            TransactionReference = orderId
        };
        var apiResponse = await _mediator.Send(query);
        if (apiResponse)
        {
            return Redirect("https://quanly-dimpos.web.app/cancel");
        }
        return Redirect("https://quanly-dimpos.web.app/failed");
    }  
}