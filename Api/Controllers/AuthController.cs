using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Extensions;

namespace api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var expectedUser = "API_USER".ObterVariavel();
            var expectedPass = "API_PASSWORD".ObterVariavel();

            if (login.Username != expectedUser || login.Password != expectedPass)
                return Unauthorized(new { mensagem = "Credenciais inválidas." });

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username)
            };

            var jwtSecret = "JWT_SECRET".ObterVariavel();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
