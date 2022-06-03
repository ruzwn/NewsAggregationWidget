using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewsAggregationWidget.Core;
using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Helpers;

namespace NewsAggregationWidget.Authorization;

public class JwtUtils : IJwtUtils
{
	private readonly INHibernateRepository _repository;
	private readonly AppSettings _appSettings;
	// private readonly ILogger _logger;

	public JwtUtils(INHibernateRepository repository, IOptions<AppSettings> appSettings)
	{
		_repository = repository;
		_appSettings = appSettings.Value;
	}

	public string GenerateJwtToken(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[] {new Claim("id", user.Id.ToString())}),
			Expires = DateTime.UtcNow.AddMinutes(1),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(token);
	}

	public Guid? ValidateJwtToken(string token)
	{
		if (token == null)
		{
			return null;
		}

		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
		try
		{
			tokenHandler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken) validatedToken;
			var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

			return userId;
		}
		catch
		{
			// todo: add logger
			return null;
		}
	}

	public RefreshToken GenerateRefreshToken(string ipAddress)
	{
		var refreshToken = new RefreshToken
		{
			Id = Guid.NewGuid(),
			Token = GetUniqueToken(),
			Expires = DateTime.UtcNow.AddDays(7),
			Created = DateTime.UtcNow,
			CreatedByIp = ipAddress
		};

		return refreshToken;

		string GetUniqueToken()
		{
			while (true)
			{
				var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
				var tokenIsUnique = !_repository.GetAll().Any(u => u.RefreshTokens.Any(t => t.Token == token));

				if (!tokenIsUnique)
				{
					continue;
				}
				
				return token;
			}
		}
	}
}