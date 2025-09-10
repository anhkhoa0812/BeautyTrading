using System.Security.Claims;

namespace BT.Application.Common.Utils;

public static class JwtUtil
{
    public static string GetCurrentAccountId(ClaimsIdentity identity)
    {
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return userClaims.FirstOrDefault(x => x.Type == "AccountId")?.Value;
        }
        return null;
    }
    public static string GetRole(ClaimsIdentity identity)
    {
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        }
        return null;
    }
    public static string GetCurrentUsername(ClaimsIdentity identity)
    {
        if (identity != null)
        {
            var userClaims = identity.Claims;
            return userClaims.FirstOrDefault(x => x.Type == "Username")?.Value;
        }
        return null;
    }
}