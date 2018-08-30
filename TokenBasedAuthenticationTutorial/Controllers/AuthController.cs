using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenBasedAuthenticationTutorial.Data;
using TokenBasedAuthenticationTutorial.Models;

namespace TokenBasedAuthenticationTutorial.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public AuthController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn([FromBody] LogInModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyVerylongSuperSecureKey"));

                var token = new JwtSecurityToken(
                    issuer: "http//taps.com",
                    audience: "http//taps.com",
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo

                });


            }
            return Unauthorized();
        }
    }
}