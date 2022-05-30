using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Helpers;

public static class UserHelper
{
	public static string GenerateJwt(this IConfiguration configuration, User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		// key for encryption (or encoding) (encoding, enctription and hashing are different things)
		var key = Encoding.ASCII.GetBytes(configuration["Secret"]);
		// header, payload and signature of jwt
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
			Expires = DateTime.UtcNow.AddMinutes(10),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		
		return tokenHandler.WriteToken(token);
	}
}