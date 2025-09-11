using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BT.Application.Services.Interface;
using BT.Domain.Entities;
using BT.Domain.Enums;
using BT.Domain.Models.Settings;
using BT.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BT.Application.Services.Implement;

public class AuthenticationService : IAuthenticationService
{
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }
    public string GenerateAccessToken(Account account)
    {
        var tokenhandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey!);
        var timeExpire = TimeUtil.GetCurrentSEATime().AddDays((double)_jwtSettings.TokenExpiry!);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim("AccountId", account.Id.ToString()),
                    new Claim("Username", account.Username),
                    new Claim(ClaimTypes.Role, account.Role.ToString())
                }
            ),
            Expires = timeExpire,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
        };
        var token = tokenhandler.CreateToken(tokenDescriptor);
        var tokenString = tokenhandler.WriteToken(token);
        return tokenString;
    }
}