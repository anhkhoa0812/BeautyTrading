using BT.Application.Common.Utils;
using BT.Application.Features.Accounts.Command.CreateAccount;
using BT.Application.Features.Authentication.Command.Login;
using BT.Domain.Constants;
using BT.Domain.Models.Accounts;
using BT.Domain.Models.Authentication.Login;
using BT.Domain.Models.Common;
using Mediator;
using Microsoft.AspNetCore.Mvc;

namespace BT.Application.Controllers;

[ApiController]
[Route(ApiEndPointConstant.Authentication.AuthenticationEndpoint)]
public class AuthenticationController : BaseController<AuthenticationController>
{
    private readonly IMediator _mediator;
    private readonly ValidationUtil<LoginCommand> _loginValidationUtil;
    private readonly ValidationUtil<CreateAccountCommand> _createAccountValidationUtil;
    
    public AuthenticationController(ILogger logger, IMediator mediator,
        ValidationUtil<LoginCommand> loginValidationUtil,
        ValidationUtil<CreateAccountCommand> createAccountValidationUtil) : base(logger)
    {
        _mediator = mediator;
        _loginValidationUtil = loginValidationUtil;
        _createAccountValidationUtil = createAccountValidationUtil;
    }

    [HttpPost(ApiEndPointConstant.Authentication.Login)]
    [ProducesResponseType<ApiResponse<LoginResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var (isValid, response) = await _loginValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }

        var apiResponse = await _mediator.Send(command);
        return Ok(apiResponse);
    } 
    [HttpPost(ApiEndPointConstant.Authentication.Register)]
    [ProducesResponseType<ApiResponse<CreateAccountResponse>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ApiResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CreateAccountCommand command)
    {
        var (isValid, response) = await _createAccountValidationUtil.ValidateAsync(command);
        if (!isValid)
        {
            return BadRequest(response);
        }
        
        var apiResponse = await _mediator.Send(command);
        return CreatedAtAction(nameof(Register), apiResponse);
    }
}