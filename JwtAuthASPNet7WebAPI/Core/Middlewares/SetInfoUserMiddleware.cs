using JwtAuthASPNet7WebAPI.Core.Contexts;
using JwtAuthASPNet7WebAPI.Core.OrtherObjects;
using JwtAuthASPNet7WebAPI.Core.Utils;
using System.Security.Claims;

namespace JwtAuthASPNet7WebAPI.Core.Middlewares
{
    public class SetInfoUserMiddleware
    {
        private readonly RequestDelegate _next;

        public SetInfoUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Here, we're simulating retrieving user info. 
            // In real scenarios, you might extract it from a token, a claim, or a header.
            string name = context.User?.Identity?.Name ?? CConstants.SystemUser;
            string id = context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            string role = context.User?.FindFirstValue(ClaimTypes.Role) ?? "";
            string email = context.User?.FindFirstValue(ClaimTypes.Email) ?? "";
            string mobilePhone = context.User?.FindFirstValue(ClaimTypes.MobilePhone) ?? "";

            // Store user info in AsyncLocal, which is thread-safe and scoped to the request
            UserContext.SetUserData(new Dtos.CurrentUser()
            {
                Name = name,
                Email = email,
                PhoneNumber = mobilePhone,
                Id = Guid.TryParse(id, out var idReturn) ? idReturn : Guid.Empty,
                Roles = role?.SplitRoles() ?? [RoleType.Guest],
            });

            // Call the next middleware in the pipeline
            await _next(context);

            // Optionally clear the value after the request
            UserContext.ClearUserData();
        }
    }
}
