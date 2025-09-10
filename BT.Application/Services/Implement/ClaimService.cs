using System.Security.Claims;
using BT.Application.Common.Utils;
using BT.Application.Services.Interface;

namespace BT.Application.Services.Implement;

public class ClaimService : IClaimService
{
    public ClaimService(IHttpContextAccessor httpContextAccessor)
    {
        var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        var accountId = Guid.TryParse(JwtUtil.GetCurrentAccountId(identity), out var accountIdResult ) ? accountIdResult : Guid.Empty;
        var username = JwtUtil.GetCurrentUsername(identity);
        var role = JwtUtil.GetRole(identity);
        GetCurrentUserId = accountId == Guid.Empty ? Guid.Empty : accountId;
        GetCurrentUsername = string.IsNullOrEmpty(username) ? "" : username;
        GetRole = string.IsNullOrEmpty(role) ? string.Empty : role;
    }
    
    public Guid GetCurrentUserId { get; }
    public string GetCurrentUsername { get; }
    public string GetRole { get; }
}