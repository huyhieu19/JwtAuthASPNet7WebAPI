using JwtAuthASPNet7WebAPI.Core.Contexts;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtAuthASPNet7WebAPI.Core.CusAuthorizeAttribute
{
    public class CusAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly RoleType _roleType;

        public CusAuthorizeAttribute(RoleType roleType)
        {
            _roleType = roleType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
                return;
            }

            if (!UserContext.Current.User.Roles.Any(p => p >= _roleType))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                return;
            }

            //if (_id != Guid.Empty && _id != UserContext.Current.User.Id)
            //{
            //    context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
            //    return;
            //}
        }
    }
}
