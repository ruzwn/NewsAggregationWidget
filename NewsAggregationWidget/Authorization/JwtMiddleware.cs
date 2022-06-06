using NewsAggregationWidget.Services;

namespace NewsAggregationWidget.Authorization;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;

	public JwtMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
	{
		// var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		// Console.WriteLine(token);
		// var userId = jwtUtils.ValidateJwtToken(token);
		// if (userId != null)
		// {
		// 	context.Items["User"] = userService.GetById(userId.Value);
		// }

		// not secure version ?
		if (context.Request.Cookies.TryGetValue("access_token", out var token))
		{
			var userId = jwtUtils.ValidateJwtToken(token);
		
			if (userId != null)
			{
				context.Items["User"] = userService.GetById(userId.Value);
			}
		}

		await _next(context);
	}
}