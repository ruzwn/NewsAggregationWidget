using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Authorization;

public interface IJwtUtils
{
	public string GenerateJwtToken(User user);

	public Guid? ValidateJwtToken(string token);

	public RefreshToken GenerateRefreshToken(string ipAddress);
}