using BT.Domain.Models.Common;
using Mediator;

namespace BT.Application.Features.Authentication.Command.Login;

public class LoginCommand : IRequest<ApiResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
}