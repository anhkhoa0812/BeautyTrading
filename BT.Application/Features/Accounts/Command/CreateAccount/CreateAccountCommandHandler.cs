using BT.Application.Common.Utils;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Accounts;
using BT.Domain.Models.Common;
using BT.Infrastructure.Persistence;
using BT.Infrastructure.Repositories.Interface;
using Mediator;

namespace BT.Application.Features.Accounts.Command.CreateAccount;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ApiResponse>
{
    private readonly IUnitOfWork<BeautyTradingContext> _unitOfWork;
    private readonly ILogger _logger;
    private readonly IAuthenticationService _authenticationService;
    public CreateAccountCommandHandler(IUnitOfWork<BeautyTradingContext> unitOfWork, ILogger logger,
        IAuthenticationService authenticationService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _authenticationService = authenticationService;
    }
    
    public async ValueTask<ApiResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await _unitOfWork.GetRepository<Account>().SingleOrDefaultAsync(
            predicate: acc => acc.Username == request.Username 
                              ||  acc.PhoneNumber == request.PhoneNumber
        );
        if (existingAccount != null)
        {
            throw new BadHttpRequestException("Username or Phone number already exists");
        }

        var newAccount = new Account()
        {
            Id = Guid.CreateVersion7(),
            Username = request.Username,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Role = ERole.User
        };
        newAccount.PasswordHash = PasswordUtil.HashPassword(request.Password);
        
        await _unitOfWork.GetRepository<Account>().InsertAsync(newAccount);
        var isSuccess = await _unitOfWork.CommitAsync() > 0;
        if (!isSuccess)
        {
            throw new Exception("Create account failed");
        }

        var token = _authenticationService.GenerateAccessToken(newAccount);

        var response = new CreateAccountResponse()
        {
            Username = newAccount.Username,
            AccountId = newAccount.Id,
            AccessToken = token
        };

        return new ApiResponse()
        {
            Status = StatusCodes.Status201Created,
            Data = response,
            Message = "Create account successfully"
        };
    }
}