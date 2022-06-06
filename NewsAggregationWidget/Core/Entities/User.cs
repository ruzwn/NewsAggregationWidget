using System.Text.Json.Serialization;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Core.Entities;

// This is an object(entity). We get it from the database and work with in all program?
public class User
{
	public virtual Guid Id { get; set; }
	public virtual string FirstName { get; set; }
	public virtual string LastName { get; set; }
	public virtual string UserName { get; set; }
	public virtual string Email { get; set; }

	[JsonIgnore]
	public virtual string Password { get; set; } // todo: change string password to some hash

	[JsonIgnore]
	public virtual IList<RefreshToken> RefreshTokens { get; set; }

	[JsonIgnore] 
	public virtual IList<News> NewsList { get; set; }

	public User()
	{
		RefreshTokens = new List<RefreshToken>();
		NewsList = new List<News>();
	}

	public User(RegisterUser model)
	{
		Id = Guid.NewGuid();
		FirstName = model.FirstName;
		LastName = model.LastName;
		UserName = model.UserName;
		Email = model.Email;
		Password = model.Password;
		RefreshTokens = new List<RefreshToken>();
		NewsList = new List<News>();
	}
}