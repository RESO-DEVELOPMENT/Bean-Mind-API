using Bean_Mind_Data.Enums;
using Bean_Mind.API.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Bean_Mind.API.Validators
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(params RoleEnum[] roleEnums)
        {
            var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
            Roles = string.Join(",", allowedRolesAsString);
        }
    }
}
