using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Accounts.Command.CreateAccount;

public class CreateAccountCommand : IRequest<ApiResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}