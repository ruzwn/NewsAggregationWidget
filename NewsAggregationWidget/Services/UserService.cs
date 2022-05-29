using NewsAggregationWidget.Core;
using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget;

public class UserService : IUserService
{
	private readonly INHibernateRepository _userRepository;
	private readonly IConfiguration _configuration;

	public UserService(INHibernateRepository repository, IConfiguration configuration)
	{
		_userRepository = repository;
		_configuration = configuration;
	}

	public AuthenticateResponse Authenticate(AuthenticateRequest authModel)
	{
		var user = _userRepository
			.GetAll()
			.FirstOrDefault(u => u.UserName == authModel.UserName && u.Password == authModel.Password);
		
		if (user == null)
		{
			// todo: add logger
			return null;
		}

		var token = _configuration.GenerateJwt(user);

		return new AuthenticateResponse(user, token);
	}

	public async Task<AuthenticateResponse> Register(UserModel userModel)
	{
		// todo: add auto mapper ???
		var user = new User(userModel);
		
		var id = await _userRepository.Add(user);

		var response = Authenticate(new AuthenticateRequest {UserName = user.UserName, Password = user.Password});

		return response;
	}

	public IEnumerable<User> GetAll()
	{
		return _userRepository.GetAll();
	}

	public User GetById(Guid id)
	{
		return _userRepository.GetById(id);
	}
}