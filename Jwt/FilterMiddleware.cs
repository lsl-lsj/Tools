using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Jwt
{
    public class FilterMiddleware
    {
        private readonly RequestDelegate _next;
        public FilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string value = string.Empty;
            var cookies = context.Request?.Cookies?.TryGetValue("Authorization", out value);
            context.Request?.Headers?.Append("Authorization", "Bearer " + value);
            await _next(context);
        }
    }
}