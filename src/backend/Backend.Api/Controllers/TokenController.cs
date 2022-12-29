using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Api.Controllers
{
    public class User
    {
        public string Name { get; set; } = string.Empty;
    }

    [AllowAnonymous]
    [ApiController]
    [Route("token")]
    public class TokenController: ControllerBase
	{
        private readonly string _key;

        public TokenController(IConfiguration configuration)
		{
            _key = configuration.GetValue<string>("JwtSecret")!;
        }

        [HttpPost]
        public IActionResult CreateToken([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Name))
                return BadRequest();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name)
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(tokenHandler.WriteToken(token));
        }
	}
}

