using Microsoft.AspNetCore.Mvc;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Controllers;

// ApiController for REST-full API and Controller for WebApp ??? Which choose ???
[ApiController] // ApiController or Controller ???
[Route("[controller]")]
public class UserController : ControllerBase // Controller supports View, but ConrollerBase doesn't
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("authenticate")]
	public IActionResult Authenticate(AuthenticateRequest authModel)
	{
		var response = _userService.Authenticate(authModel);

		if (response == null)
		{
			return BadRequest(new {message = "Username or password is incorrect"});
		}

		return Ok(response);
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(UserModel userModel)
	{
		var response = await _userService.Register(userModel);

		if (response == null)
		{
			return BadRequest(new {message = "Didn't register!"});
		}

		return Ok(response);
	}

	// todo: use another class for response without password and another secret info
	[Authorize]
	[HttpGet]
	public IActionResult GetAll()
	{
		var users = _userService.GetAll();

		return Ok(users);
	}

	[Authorize]
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
}