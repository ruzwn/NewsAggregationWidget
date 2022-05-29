using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget;

public interface IUserService
{
	AuthenticateResponse Authenticate(AuthenticateRequest authModel);
	Task<AuthenticateResponse> Register(UserModel userModel);
	IEnumerable<User> GetAll();
	User GetById(Guid id);
}