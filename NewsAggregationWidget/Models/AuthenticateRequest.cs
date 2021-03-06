using System.ComponentModel.DataAnnotations;

namespace NewsAggregationWidget.Models;

public class AuthenticateRequest
{
	[Required]
	public string UserName { get; set; }
	
	[Required]
	public string Password { get; set; }
}