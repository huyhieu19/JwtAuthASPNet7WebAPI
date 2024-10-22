using System.Security.Claims;

namespace JwtAuthASPNet7WebAPI.Core.Middlewares
{
    public class UserInfo
    {
        // Use AsyncLocal to store user-specific information in a static way
        private static AsyncLocal<string> _Name = new AsyncLocal<string>();
        private static AsyncLocal<string> _Id = new AsyncLocal<string>();
        private static AsyncLocal<string> _Role = new AsyncLocal<string>();
        private static AsyncLocal<string> _Email = new AsyncLocal<string>();

        // Property to access the stored user information
        public static string Name
        {
            get => _Name.Value;
            set => _Name.Value = value;
        }
        public static string Id
        {
            get => _Id.Value;
            set => _Id.Value = value;
        }
        public static string Role
        {
            get => _Role.Value;
            set => _Role.Value = value;
        }
        public static string Email
        {
            get => _Email.Value;
            set => _Email.Value = value;
        }

        private readonly RequestDelegate _next;

        public UserInfo(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Here, we're simulating retrieving user info. 
            // In real scenarios, you might extract it from a token, a claim, or a header.
            string name = context.User?.Identity?.Name ?? "Anonymous";
            string id = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
            string role = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";
            string email = context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "";

            // Store user info in AsyncLocal, which is thread-safe and scoped to the request
            Name = name;
            Id = id;
            Role = role;
            Email = email;


            // Call the next middleware in the pipeline
            await _next(context);

            // Optionally clear the value after the request
            Name = null;
            Id = null;
            Role = null;
            Email = null;
        }
    }
}
