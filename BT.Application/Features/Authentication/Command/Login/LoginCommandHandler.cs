using BT.Application.Common.Exceptions;
using BT.Application.Common.Utils;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Models.Authentication.Login;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using BT.Infrastructure.Utils;
using Mediator;

namespace BT.Application.Features.Authentication.Command.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IAuthenticationService _authenticationService;

    public LoginCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork,
        ILogger logger, IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authenticationService =
            authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }
    
    public async ValueTask<ApiResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {nameof(LoginCommandHandler)} - {TimeUtil.GetCurrentSEATime()}");
        var account = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: x => x.Username == request.Username
        );
        if (account == null)
        {
            throw new NotFoundException("Not found account");
        }

        var hashRequestPassword = PasswordUtil.HashPassword(request.Password);
        if (!account.PasswordHash.Equals(hashRequestPassword))
        {
            throw new BadHttpRequestException("Invalid username or password");
        }

        var token = _authenticationService.GenerateAccessToken(account);
        
        _logger.Information($"END: {nameof(LoginCommandHandler)} - {TimeUtil.GetCurrentSEATime()}");
        return new ApiResponse()
        {
            Status = 200,
            Message = "Login success",
            Data = new LoginResponse()
            {
                AccountId = account.Id,
                Username = account.Username,
                AccessToken = token
            }
        };
    }
}