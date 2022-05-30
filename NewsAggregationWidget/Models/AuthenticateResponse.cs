using System.Text.Json.Serialization;
using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Models;

public class AuthenticateResponse
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string UserName { get; set; }
	public string Email { get; set; }
	public string JwtToken { get; set; }
	
	[JsonIgnore]
	public string RefreshToken { get; set; }

	public AuthenticateResponse(User user, string jwtToken, string refreshToken)
	{
		Id = user.Id;
		FirstName = user.FirstName;
		LastName = user.LastName;
		UserName = user.UserName;
		Email = user.Email;
		JwtToken = jwtToken;
		RefreshToken = refreshToken;
	}
}