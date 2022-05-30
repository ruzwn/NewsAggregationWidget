using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Services;

public interface IUserService
{
	Task<AuthenticateResponse> Authenticate(AuthenticateRequest authModel, string ipAddress);
	Task<AuthenticateResponse> Register(RegisterUser model, string ipAdrress);
	AuthenticateResponse RefreshToken(string token, string ipAddress);
	void RevokeToken(string token, string ipAddress);
	IEnumerable<User> GetAll();
	User GetById(Guid id);
}