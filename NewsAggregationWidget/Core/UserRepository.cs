using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public class UserRepository : INHibernateRepository<User>
{
	private readonly IMapperSession _session;

	public UserRepository(IMapperSession session)
	{
		_session = session;
	}
	
	public IEnumerable<User> GetAll()
	{
		return _session.Users.ToList();
	}

	public User GetById(Guid id)
	{
		var user = _session.Users.FirstOrDefault(user => user.Id == id);

		if (user == null)
		{
			// todo: log ...
		}

		return user;
	}

	public Guid Add(User token)
	{
		_session.BeginTransaction();
		var id = _session.SaveOrUpdateUser(token);
		_session.Commit();
		_session.CloseTransaction();

		return id;
	}

	public void Update(User token)
	{
		_session.BeginTransaction();
		_session.SaveOrUpdateUser(token);
		_session.Commit();
		_session.CloseTransaction();
	}

	public void Delete(User user)
	{
		_session.BeginTransaction();
		_session.DeleteUser(user);
		_session.Commit();
		_session.CloseTransaction();
	}
}