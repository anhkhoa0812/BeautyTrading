using BT.Application.Common.Utils;
using BT.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BT.Application.Common.Validators;

public class CustomAuthorizeAttribute : AuthorizeAttribute
{
    public CustomAuthorizeAttribute(params ERole[] roleEnums)
    {
        var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
        Roles = string.Join(",", allowedRolesAsString);
    }
}