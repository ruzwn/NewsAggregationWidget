using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Core.Entities;

// This is an object(entity). We get it from the database and work with in all program?
public class User : BaseEntity
{
	public virtual string FirstName { get; set; }
	public virtual string LastName { get; set; }
	public virtual string? MiddleName { get; set; }
	public virtual string UserName { get; set; }
	public virtual string Email { get; set; }
	public virtual string Password { get; set; } // todo: change string password to some hash

	public User() { }

	public User(UserModel model)
	{
		Id = Guid.NewGuid();
		FirstName = model.FirstName;
		LastName = model.LastName;
		MiddleName = model.MiddleName;
		UserName = model.UserName;
		Email = model.Email;
		Password = model.Password;
	}
}