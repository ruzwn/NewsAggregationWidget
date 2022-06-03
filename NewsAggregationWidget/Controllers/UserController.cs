using System.Net;
using Microsoft.AspNetCore.Mvc;
using NewsAggregationWidget.Authorization;
using NewsAggregationWidget.Models;
using NewsAggregationWidget.Services;

namespace NewsAggregationWidget.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
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
		
		SetTokenCookie(response.RefreshToken);
		
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
		var refreshToken = Request.Cookies["refreshToken"];

		if (string.IsNullOrEmpty(refreshToken))
		{
			return BadRequest("You don't have refresht token");
		}
		
		var response = _userService.RefreshToken(refreshToken, IpAddress());
		
		SetTokenCookie(response.RefreshToken);
		
		return Ok(response);
	}

	[AllowAnonymous]
	[HttpPost("revoke-token")]
	public IActionResult RevokeToken(RevokeTokenRequest model)
	{
		var token = model.Token ?? Request.Cookies["refreshToken"];

		if (string.IsNullOrEmpty(token))
		{
			return BadRequest(new {message = "Token is required"});
		}

		token = WebUtility.UrlDecode(token);
		_userService.RevokeToken(token, IpAddress());
		
		return Ok(new { message = $"Token {token} revoked" });
	}
	
	[HttpGet]
	public IActionResult GetAll()
	{
		var users = _userService.GetAll();

		return Ok(users);
	}
	
	[HttpGet("{id:guid}")]
	public IActionResult GetById([FromRoute] Guid id)
	{
		var user = _userService.GetById(id);

		if (user == null)
		{
			return NotFound($"User with id: {id} not found!");
		}

		return Ok(user);
	}
	
	private void SetTokenCookie(string token)
	{
		var cookieOptions = new CookieOptions
		{
			HttpOnly = true,
			Expires = DateTime.UtcNow.AddDays(7)
		};
		
		Response.Cookies.Append("refreshToken", token, cookieOptions);
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