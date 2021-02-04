using System.Text;
using System.Collections.Generic;
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
            context.Request?.Cookies?.TryGetValue("JWT_SESSION_KEY", out value);
            // context.Request?.Headers?.Append("Authorization", "Bearer " + value);

            // byte[] bytes = null;
            // context.Session.TryGetValue("", out bytes);

            if (!string.IsNullOrWhiteSpace(value))
            {
                value = context.Session.GetString(value);
            }
            // context.Request?.Headers?.Append("Authorization", "Bearer " + value);
            // var values = Encoding.UTF8.GetString(bytes);
            // if(context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            // {
            //     context.Response.Redirect("/api/login");
            //     await _next(context);
            // }
            await _next(context);
        }
    }
}