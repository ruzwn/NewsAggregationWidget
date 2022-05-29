﻿using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NewsAggregationWidget;

public class JwtMiddleware
{
	private readonly RequestDelegate _next;

	private readonly IConfiguration _configuration;
	// private readonly ILogger _logger;

	public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
	{
		_next = next;
		_configuration = configuration;
	}

	public async Task Invoke(HttpContext context, IUserService userService)
	{
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (token != null)
		{
			AttachUserToContext(context, userService, token);
		}

		await _next(context);
	}

	public void AttachUserToContext(HttpContext context, IUserService userService, string token)
	{
		try
		{
			// trying validate token received from request
			var tokenHandler = new JwtSecurityTokenHandler();
			// min 16 characters
			var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
			tokenHandler.ValidateToken(
				token,
				new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ClockSkew = TimeSpan.Zero
				},
				out SecurityToken validatedToken
			);

			var jwtToken = (JwtSecurityToken) validatedToken;
			var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

			context.Items["User"] = userService.GetById(userId);
		}
		catch
		{
			// todo: add logger
		}
	}
}