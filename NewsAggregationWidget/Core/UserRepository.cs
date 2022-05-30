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

	public async Task<Guid> Add(User user)
	{
		_session.BeginTransaction();
		var result = await _session.Save(user);
		await _session.Commit();
		_session.CloseTransaction();

		return result;
	}

	public async Task Update(User entity)
	{
		_session.BeginTransaction();
		await _session.Save(entity);
		await _session.Commit();
		_session.CloseTransaction();
	}
	
	
	// todo: separate this repo into 2 different repo (one for user and other for token) ???

	public async Task<Guid> AddToken(RefreshToken token)
	{
		_session.BeginTransaction();
		var result = await _session.SaveToken(token);
		await _session.Commit();
		_session.CloseTransaction();

		return result;
	}

	public async Task UpdateToken(RefreshToken token)
	{
		_session.BeginTransaction();
		await _session.SaveToken(token);
		await _session.Commit();
		_session.CloseTransaction();
	}
}