using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public class UserRepository : INHibernateRepository
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
		var result = _session.Users.FirstOrDefault(user => user.Id == id);
		
		if (result == null)
		{
			// todo: add logger
			return null;
		}

		return result;
	}

	public Guid Add(User user)
	{
		_session.BeginTransaction();
		var result = _session.SaveOrUpdate(user);
		_session.Commit();
		_session.CloseTransaction();

		return result;
	}

	public void Update(User entity)
	{
		_session.BeginTransaction();
		_session.SaveOrUpdate(entity);
		_session.Commit();
		_session.CloseTransaction();
	}
	
	
	// todo: separate this repo into 2 different repo (one for user and other for token) ???

	public Guid AddToken(RefreshToken token)
	{
		_session.BeginTransaction();
		var result = _session.SaveOrUpdateToken(token);
		_session.Commit();
		_session.CloseTransaction();

		return result;
	}

	public void UpdateToken(RefreshToken token)
	{
		_session.BeginTransaction();
		_session.SaveOrUpdateToken(token);
		_session.Commit();
		_session.CloseTransaction();
	}
}