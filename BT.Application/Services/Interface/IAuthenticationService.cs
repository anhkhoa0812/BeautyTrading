using BT.Domain.Entities;
using BT.Domain.Enums;

namespace BT.Application.Services.Interface;

public interface IAuthenticationService
{
    string GenerateAccessToken(Account account);
}