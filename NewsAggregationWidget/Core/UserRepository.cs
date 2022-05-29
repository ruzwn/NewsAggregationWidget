using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public class UserRepository : INHibernateRepository
{
	private readonly IMapperSession _session;

	public UserRepository(IMapperSession session)
	{
		_session = session;
	}
	
	public List<User> GetAll()
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

	public async Task<Guid> Add(User entity)
	{
		_session.BeginTransaction();
		var result = await _session.Save(entity);
		await _session.Commit();
		_session.CloseTransaction();

		return result;
	}
}