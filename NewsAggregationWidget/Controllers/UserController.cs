using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsAggregationWidget.Authorization;
using NewsAggregationWidget.Models;
using NewsAggregationWidget.Services;

namespace NewsAggregationWidget.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[AllowAnonymous]
	[HttpPost("authenticate")]
	public IActionResult Authenticate(AuthenticateRequest authModel)
	{
		var response = _userService.Authenticate(authModel, IpAddress());

		if (response == null)
		{
			return BadRequest(new {message = "Incorrect user or password"});
		}
		
		SetTokenCookie(response.JwtToken, response.RefreshToken);
		
		return Ok(response);
	}
	
	[AllowAnonymous]
	[HttpPost("register")]
	public IActionResult Register(RegisterUser registerUser)
	{
		var response = _userService.Register(registerUser, IpAddress());

		if (response == null)
		{
			return BadRequest(new {message = "Registration required"});
		}

		return Ok(response);
	}
	
	[AllowAnonymous]
	[HttpPost("refresh-token")]
	public IActionResult RefreshToken()
	{
		//  todo: use this api, when user have refresh token and access token expired
		
		var refreshToken = Request.Cookies["refresh_token"];

		if (string.IsNullOrEmpty(refreshToken))
		{
			return BadRequest("You don't have refresht token");
		}
		
		refreshToken = WebUtility.UrlDecode(refreshToken);
		var response = _userService.RefreshToken(refreshToken, IpAddress());
		
		SetTokenCookie(response.JwtToken, response.RefreshToken);
		
		return Ok(response);
	}

	[AllowAnonymous]
	[HttpPost("revoke-token")]
	public IActionResult RevokeToken(RevokeTokenRequest model)
	{
		var refreshToken = model.Token ?? Request.Cookies["refresh_token"];

		if (string.IsNullOrEmpty(refreshToken))
		{
			return BadRequest(new {message = "Token is required"});
		}

		refreshToken = WebUtility.UrlDecode(refreshToken);
		_userService.RevokeToken(refreshToken, IpAddress());
		
		return Ok(new { message = $"Token {refreshToken} revoked" });
	}
	
	[HttpGet]
	public IActionResult GetAll()
	{
		var users = _userService.GetAll();

		return Ok(users);
	}
	
	[HttpGet("{id:guid}")]
	public IActionResult GetById(Guid id)
	{
		var user = _userService.GetById(id);

		if (user == null)
		{
			return NotFound($"User with id: {id} not found!");
		}

		return Ok(user);
	}
	
	private void SetTokenCookie(string accessToken, string refreshToken)
	{
		var accessTokenCookieOptions = new CookieOptions
		{
			Expires = DateTime.UtcNow.AddMinutes(20)
		};
		Response.Cookies.Append("access_token", accessToken, accessTokenCookieOptions);
		
		var refresthTokenCookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = DateTime.UtcNow.AddDays(7)
		};
		Response.Cookies.Append("refresh_token", refreshToken, refresthTokenCookieOptions);
	}

	private string IpAddress()
	{
		if (Request.Headers.ContainsKey("X-Forwarded-For"))
		{
			return Request.Headers["X-Forwarded-For"];
		}

		return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
	}
}