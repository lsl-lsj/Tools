using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Jwt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Jwt.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        [Route("/api/login")]
        [AllowAnonymous]
        public IActionResult Login(string userName, string pwd)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pwd))
            {
                var json = JsonSerializer.Serialize(new UserLoginModel { Password = pwd, UserName = userName });
                var sessionKey = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                // cookies.Delete("Authorization");
                HttpContext.Response.Cookies.Append("JWT_SESSION_KEY", sessionKey, new CookieOptions
                {
                    Expires = DateTime.Now.AddSeconds(1000)
                });
                var token = TokenService.CreateAndRefreshToken(HttpContext, sessionKey);
                return Ok(new
                {
                    token = token
                });
            }
            else
            {
                return BadRequest(new { message = "username or password is incorrect." });
            }
        }

        [HttpGet]
        [Route("api/get")]
        //JWT验证标识
        [Authorize] //(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)
        public ActionResult<IEnumerable<string>> Get()
        {
            var token = TokenService.CreateAndRefreshToken(HttpContext);
            return new string[] { "value1", "value2", token };
        }
    }
}