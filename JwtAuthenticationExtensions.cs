using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace UMS1._0;
public  class JwtAuthenticationExtensions
{
	private readonly IConfiguration _configuration;

	public JwtAuthenticationExtensions(IConfiguration configuration)
	{
		_configuration = configuration;
	}
	
	public void AddJwtAuthentication(IServiceCollection services)
	{
		// Извлечение настроек из appsettings
		var jwtKey = _configuration["Jwt:Key"];
		var issuer = _configuration["Jwt:Issuer"];
		var audience = _configuration["Jwt:Audience"];

		// Настройка аутентификации JWT
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = issuer,
					ValidAudience = audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
				};
			});
	}
}
