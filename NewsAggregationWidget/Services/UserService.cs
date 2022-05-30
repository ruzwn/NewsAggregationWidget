using Microsoft.Extensions.Options;
using NewsAggregationWidget.Authorization;
using NewsAggregationWidget.Core;
using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Helpers;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Services;

public class UserService : IUserService
{
	private readonly INHibernateRepository _userRepository;
	private readonly IJwtUtils _jwtUtils;
	private readonly AppSettings _appSettings;

	public UserService(INHibernateRepository repository, IJwtUtils jwtUtils, IOptions<AppSettings> appSettings)
	{
		_userRepository = repository;
		_jwtUtils = jwtUtils;
		_appSettings = appSettings.Value;
	}

	public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authModel, string ipAddress)
	{
		var user = _userRepository
			.GetAll()
			.FirstOrDefault(u => u.UserName == authModel.UserName && u.Password == authModel.Password);

		if (user == null)
		{
			return null;
		}

		var jwtToken = _jwtUtils.GenerateJwtToken(user);
		var refreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);

		refreshToken.UserId = user.Id;
		refreshToken.User = user;
		user.RefreshTokens.Add(refreshToken);

		RemoveOldRefreshTokens(user);

		// todo: рефреш токен не сохраняется в БД !!!
		
		var tokenId = await _userRepository.AddToken(refreshToken);
		Console.Write("RefreshToken id: ");
		Console.WriteLine(tokenId.ToString());
		await _userRepository.Update(user);

		return new AuthenticateResponse(user, jwtToken, refreshToken.Token);
	}

	public async Task<AuthenticateResponse> Register(RegisterUser model, string ipAddress)
	{
		var user = new User(model);

		var id = await _userRepository.Add(user);

		var response = Authenticate(
			new AuthenticateRequest {UserName = user.UserName, Password = user.Password}, ipAddress);

		return await response;
	}

	public AuthenticateResponse RefreshToken(string token, string ipAddress)
	{
		var user = GetUserByRefreshToken(token);
		var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

		if (refreshToken.IsRevoked)
		{
			RevokeDescendantRefreshTokens(
				refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
			
			_userRepository.Update(user);
		}

		if (!refreshToken.IsActive)
		{
			throw new AppException("Invalid token");
		}
		
		var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
		user.RefreshTokens.Add(newRefreshToken);
		
		RemoveOldRefreshTokens(user);

		// todo: is update db will work correctly ???
		_userRepository.Update(user);
		
		var jwtToken = _jwtUtils.GenerateJwtToken(user);

		return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
	}

	public void RevokeToken(string token, string ipAddress)
	{
		var user = GetUserByRefreshToken(token);
		var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

		if (!refreshToken.IsActive)
		{
			throw new AppException("Invalid token");
		}
		
		RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");

		_userRepository.Update(user);
	}

	public IEnumerable<User> GetAll()
	{
		return _userRepository.GetAll();
	}

	public User GetById(Guid id)
	{
		return _userRepository.GetById(id);
	}

	private void RemoveOldRefreshTokens(User user)
	{
		// can nhibernate automatic delete tokens when we delete from user's list in c#
		// todo: how to fix .ToList() ???
		user.RefreshTokens.ToList().RemoveAll(x =>
			!x.IsActive &&
			x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
	}

	private User GetUserByRefreshToken(string token)
	{
		var user = GetAll().SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

		if (user == null)
		{
			throw new AppException("Invalid token");
		}

		return user;
	}
	
	private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
	{
		var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
		RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
		
		return newRefreshToken;
	}


	private void RevokeDescendantRefreshTokens(
		RefreshToken refreshToken, User user, string ipAddress, string reason)
	{
		if (string.IsNullOrEmpty(refreshToken.ReplacedByToken))
		{
			return;
		}

		var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
		
		if (childToken.IsActive)
		{
			RevokeRefreshToken(childToken, ipAddress, reason);
		}
		else
		{
			RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
		}
	}

	private static void RevokeRefreshToken(
		RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
	{
		token.Revoked = DateTime.UtcNow;
		token.RevokedByIp = ipAddress;
		token.ReasonRevoked = reason;
		token.ReplacedByToken = replacedByToken;
	}
}