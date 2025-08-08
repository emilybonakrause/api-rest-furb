using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmilyApiFurb.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private const string ChaveSecreta = "chave-secreta-jwt-da-emily-krause";

    [HttpPost]
    public IActionResult Post([FromForm] string usuario, [FromForm] string senha)
    {
        if (usuario != "emily" || senha != "emily")
            return Unauthorized();

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario),
            new Claim("role", "Admin")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ChaveSecreta));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "api-krause",
            audience: "api-krause",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new
        {
            access_token = new JwtSecurityTokenHandler().WriteToken(token),
            token_type = "Bearer",
            expires_in = 3600
        });
    }
}

