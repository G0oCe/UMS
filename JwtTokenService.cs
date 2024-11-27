using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace UMS1._0;

public class JwtTokenService
{
	private readonly IConfiguration _configuration;

	public JwtTokenService(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string GenerateJwtToken(string username, string role)
	{
		var jwtKey = _configuration["Jwt:Key"];
		var issuer = _configuration["Jwt:Issuer"];
		var audience = _configuration["Jwt:Audience"];

		var claims = new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, username),
			new Claim(ClaimTypes.Role, role), // Add role claim
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
			issuer: issuer,
			audience: audience,
			claims: claims,
			expires: DateTime.Now.AddMinutes(30),
			signingCredentials: creds);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

}
