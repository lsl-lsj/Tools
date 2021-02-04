using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Jwt.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Jwt
{
    public class TokenService
    {
        public static string CreateAndRefreshToken(HttpContext context, string sessionKey = null)
        {
            if (string.IsNullOrWhiteSpace(sessionKey))
            {
                context.Request.Cookies.TryGetValue("JWT_SESSION_KEY", out sessionKey);
            }
            
            if (sessionKey == null)
            {
                return string.Empty;
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(sessionKey));
            var user = JsonSerializer.Deserialize<UserLoginModel>(json);
            Const.Audience = user.UserName + user.Password + DateTime.Now.ToString();
            var claims = new[]
            {
                    // new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                    // new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("pwd", user.Password),
                    new Claim(ClaimTypes.Role,user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Const.Issuer,
                audience: Const.Audience,
                claims: claims,
                expires: DateTime.Now.AddSeconds(1000),
                signingCredentials: creds
                );

            context.Session.SetString(sessionKey, new JwtSecurityTokenHandler().WriteToken(token));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}