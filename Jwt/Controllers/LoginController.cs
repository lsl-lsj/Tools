using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Jwt.Controllers
{

    public class LoginController : Controller
    {
        [HttpGet]
        [Route("api/login")]
        [AllowAnonymous]
        public IActionResult Login(string userName, string pwd)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(pwd))
            {
                var claims = new[]
                {
                    // new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    // new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("pwd", pwd),
                    new Claim(ClaimTypes.Role,userName)
            };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: Const.Domain,
                    audience: Const.Domain,
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(5),
                    signingCredentials: creds
                    );


                var cookies = HttpContext.Response.Cookies;
                // cookies.Delete("Authorization");
                cookies.Append("Authorization", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
                {
                    Expires = DateTime.Now.AddSeconds(10)
                });
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
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
            return new string[] { "value1", "value2" };
        }
    }
}